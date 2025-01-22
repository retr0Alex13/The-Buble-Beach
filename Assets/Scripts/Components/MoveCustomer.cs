using DG.Tweening;
using UnityEngine;
using Zenject;

public class MoveCustomer : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    [SerializeField] private float swimDuration = 3f;
    [SerializeField] private float maxSwimDuration = 5f;

    [Inject(Id = "JumpPoint")] private Transform jumpPoint;
    [Inject(Id = "StartPoint")] private Transform startPoint;

    private Tween currentMoveTween;

    private void Start()
    {
        IntroSequence();
    }

    private void IntroSequence()
    {
        Sequence introSequence = DOTween.Sequence();
        introSequence.Append(transform.DOMoveX(jumpPoint.position.x, swimDuration));
        introSequence.Append(transform.DOJump(new Vector3(-2.21f, -0.3f, 0f), 2, 1, 2));
    }

    public void MoveToPosition(Vector2 position)
    {
        if (IsMoving) 
            return;

        IsMoving = true;
        currentMoveTween?.Kill();

        float randomSwimDuration = Random.Range(swimDuration, maxSwimDuration);

        currentMoveTween = transform.DOMove(position, randomSwimDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            IsMoving = false;
            currentMoveTween = null;
        });
    }

    public void LeaveBeach()
    {
        currentMoveTween?.Kill();

        Sequence leaveSequence = DOTween.Sequence();
        leaveSequence.Append(transform.DOMove(jumpPoint.position, swimDuration));
        leaveSequence.Append(transform.DOMove(startPoint.position, swimDuration));
    }
}
