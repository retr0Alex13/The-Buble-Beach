using DG.Tweening;
using UnityEngine;

public class MoveCustomer : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    private float swimDuration = 4f; // Make it Random

    private Tween currentMoveTween;

    private void Start()
    {
        IntroSequence();
    }

    private void IntroSequence()
    {
        Sequence introSequence = DOTween.Sequence();
        introSequence.Append(transform.DOMoveX(3.8f, swimDuration));
        introSequence.Append(transform.DOJump(new Vector3(-2.21f, -0.3f, 0f), 2, 1, 2));
    }

    public void MoveToPosition(Vector2 position)
    {
        if (IsMoving) 
            return;

        IsMoving = true;
        currentMoveTween?.Kill();

        currentMoveTween = transform.DOMove(position, swimDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            IsMoving = false;
            currentMoveTween = null;
        });
    }
}
