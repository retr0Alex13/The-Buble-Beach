using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class MoveCustomer : MonoBehaviour
{
    [SerializeField] private float swimDuration = 3f;
    [SerializeField] private float maxSwimDuration = 5f;

    [Inject(Id = "JumpPoint")] private Transform jumpPoint;
    [Inject(Id = "JumpEndPoint")] private Transform jumpEndPoint;
    [Inject(Id = "StartPoint")] private Transform startPoint;
    [Inject(Id = "DrownLine")] private Transform drownLine;
    [SerializeField] private AudioClip drownSound;

    private CustomerSpawnManager customerManager;
    private ScoreManager scoreManager;

    private AudioSource audioSource;
    private Animator animator;

    public bool IsMoving => isMoving;
    private bool isMoving;

    private CustomerStayTime customerTime;

    private Tween currentMoveTween;

    private void Awake()
    {
        customerTime = GetComponent<CustomerStayTime>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        customerTime.OnCustomerLeave += LeaveBeach;
    }

    private void OnDestroy()
    {
        customerTime.OnCustomerLeave -= LeaveBeach;
        CancelCurrentTween();
    }

    private void Start()
    {
        customerManager = FindAnyObjectByType<CustomerSpawnManager>();
        scoreManager = FindAnyObjectByType<ScoreManager>();
        IntroSequence();
    }

    private void IntroSequence()
    {
        AdjustFacingDirection(jumpPoint.position);

        DOTween.Sequence()
            .Append(transform.DOMoveX(jumpPoint.position.x, swimDuration))
            .Append(transform.DOJump(jumpEndPoint.position, 3, 1, 2)
                .OnStart(() => {
                    animator.SetBool("Jump", true);
                })
                .OnComplete(() => {
                    animator.SetBool("Jump", false);
                }));
    }

    public void MoveToPosition(Vector2 position)
    {
        if (isMoving)
        {
            return;
        }

        isMoving = true;
        CancelCurrentTween();

        float moveDuration = Random.Range(swimDuration, maxSwimDuration);

        AdjustFacingDirection(position);

        currentMoveTween = transform.DOMove(position, moveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(OnMoveComplete)
            .OnKill(OnMoveComplete);
    }

    private void AdjustFacingDirection(Vector2 position)
    {
        float direction = Mathf.Sign(position.x - transform.position.x);
        transform.localScale = new Vector3
            (Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
    }

    private void CancelCurrentTween()
    {
        DOTween.Kill(transform);

        if (currentMoveTween != null && currentMoveTween.IsPlaying())
        {
            currentMoveTween?.Kill();
        }
        currentMoveTween = null;
    }

    private void OnMoveComplete()
    {
        isMoving = false;
        currentMoveTween = null;
    }

    public void LeaveBeach()
    {
        CancelCurrentTween();

        //Sequence leaveSequence = DOTween.Sequence();
        //leaveSequence
        //    .Append(transform.DOMove(jumpPoint.position, swimDuration))
        //    .Append(transform.DOMove(startPoint.position, maxSwimDuration))
        //    .OnComplete(() =>
        //    {
        //        DestroyCustomer();
        //    });
        StartCoroutine(LeaveBeachCoroutine());
    }

    private IEnumerator LeaveBeachCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 jumpPos = jumpPoint.position;
        Vector3 endPos = startPoint.position;

        float jumpSpeed = Vector3.Distance(startPos, jumpPos) / swimDuration; // Обчислюємо швидкість
        float returnSpeed = Vector3.Distance(jumpPos, endPos) / maxSwimDuration;

        // Переміщення до jumpPoint
        while (transform.position != jumpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, jumpPos, jumpSpeed * Time.deltaTime);
            AdjustFacingDirection(endPos);
            yield return null;
        }

        // Переміщення до startPoint
        while (transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, returnSpeed * Time.deltaTime);
            AdjustFacingDirection(endPos);
            yield return null;
        }
        scoreManager.AddPoints();
        DestroyCustomer();
    }

    public void Drown()
    {
        CancelCurrentTween();

        audioSource.PlayOneShot(drownSound);

        animator.SetBool("Dead", true);
        transform.DOMoveY(drownLine.position.y, swimDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                DestroyCustomer();
            });
    }

    private void DestroyCustomer()
    {
        CancelCurrentTween();
        customerManager.CustomerLeft(gameObject);
        Destroy(gameObject);
    }
}
