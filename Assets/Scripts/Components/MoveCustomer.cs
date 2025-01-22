using DG.Tweening;
using UnityEngine;
using Zenject;

public class MoveCustomer : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    [SerializeField] private float swimDuration = 3f;
    [SerializeField] private float maxSwimDuration = 5f;

    //[Inject(Id = "JumpPoint")] private Transform jumpPoint;
    //[Inject(Id = "StartPoint")] private Transform startPoint;
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private Transform startPoint;

    private Tween currentMoveTween;

    private void Start()
    {
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
        if (IsMoving)
        {
            return;
        }

        IsMoving = true;

        currentMoveTween?.Kill();
        currentMoveTween = null;

        float randomSwimDuration = Random.Range(swimDuration, maxSwimDuration);

        currentMoveTween = transform.DOMove(position, randomSwimDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
        {
            IsMoving = false;
            currentMoveTween = null;
        }).OnKill(() =>
        {
            IsMoving = false;
            currentMoveTween = null;
        });
    }

    public void LeaveBeach()
    {

    }
}
