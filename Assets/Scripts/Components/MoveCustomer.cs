using DG.Tweening;
using UnityEngine;

public class MoveCustomer : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    [SerializeField] private float swimDuration = 3f;
    [SerializeField] private float maxSwimDuration = 5f;

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

        float randomSwimDuration = Random.Range(swimDuration, maxSwimDuration);

        currentMoveTween = transform.DOMove(position, swimDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            IsMoving = false;
            currentMoveTween = null;
        });
    }
}
