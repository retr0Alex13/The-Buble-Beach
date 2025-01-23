using DG.Tweening;
using UnityEngine;
using Zenject;

public class MoveCustomer : MonoBehaviour
{
    [SerializeField] private float swimDuration = 3f;
    [SerializeField] private float maxSwimDuration = 5f;

    [Inject(Id = "JumpPoint")] private Transform jumpPoint;
    [Inject(Id = "StartPoint")] private Transform startPoint;
    [Inject(Id = "DrownLine")] private Transform drownLine;

    private CustomerSpawnManager customerManager;

    public bool IsMoving => isMoving;
    private bool isMoving;

    private CustomerStayTime customerTime;

    private Tween currentMoveTween;

    private void Awake()
    {
        customerTime = GetComponent<CustomerStayTime>();
        customerTime.OnCustomerLeave += LeaveBeach;
    }

    private void OnDestroy()
    {
        customerTime.OnCustomerLeave -= LeaveBeach;
    }

    private void Start()
    {
        customerManager = FindAnyObjectByType<CustomerSpawnManager>();
        IntroSequence();
    }

    private void Update()
    {
        Debug.Log($"Active tweens: {DOTween.TotalActiveTweens()}");
    }

    private void IntroSequence()
    {
        DOTween.Sequence()
            .Append(transform.DOMoveX(jumpPoint.position.x, swimDuration))
            .Append(transform.DOJump(new Vector3(-2.21f, -0.3f, 0f), 2, 1, 2))
            .SetSpeedBased(true);
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

        currentMoveTween = transform.DOMove(position, moveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(OnMoveComplete)
            .OnKill(OnMoveComplete);
    }

    private void CancelCurrentTween()
    {
        currentMoveTween?.Kill();
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
        transform.DOKill();

        DOTween.Sequence()
            .Append(transform.DOMove(jumpPoint.position, swimDuration))
            .Append(transform.DOMove(startPoint.position, maxSwimDuration))
            .SetSpeedBased(true)
            .OnKill(() =>
            {
                customerManager.CustomerLeft(gameObject);
            });
    }

    public void Drown()
    {
        CancelCurrentTween();
        transform.DOMoveY(drownLine.position.y, swimDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                customerManager.CustomerLeft(gameObject);
            });
    }
}
