using DG.Tweening;
using UnityEngine;
using Zenject;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float airAmount = 30f;
    [SerializeField] private float scale = 0.7f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float speed = 2f;

    [SerializeField] private LayerMask waterLineLayer;

    [Inject(Id = "EmergePoint")] private Transform emergePoint;

    private Sequence emergeSequence;

    void Start()
    {
        Sequence scaleSequence = DOTween.Sequence()
            .Append(transform.DOScale(scale, scaleDuration));
        scaleSequence.OnComplete(() => StartEmergeSequence());
    }

    private void OnMouseDrag()
    {
        StopEmergeSequence();
    }

    private void OnMouseUp()
    {
        StartEmergeSequence();
    }

    void Update()
    {

    }

    private void StartEmergeSequence()
    {
        emergeSequence = DOTween.Sequence()
                    .Append(transform.DOMoveY(emergePoint.position.y, speed))
                    .SetEase(Ease.InOutSine);
    }

    private void StopEmergeSequence()
    {
        emergeSequence?.Kill();
        emergeSequence = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Play sound/animation here
        if (Utils.CompareLayers(waterLineLayer, collision.gameObject.layer))
        {
            StopEmergeSequence();
            Destroy(gameObject);
        }
    }
}
