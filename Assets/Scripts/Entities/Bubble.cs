using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class Bubble : MonoBehaviour
{
    public float LifeTime => lifeTime;
    public event Action<GameObject> OnBubblePopped;

    [SerializeField] private float airAmount = 30f;
    [SerializeField] private float scale = 0.7f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float lifeTime = 10f;

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
        DragBubble();
    }

    private void DragBubble()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    private void OnMouseUp()
    {
        StartEmergeSequence();
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            PopTheBubble();
        }
    }

    private void StartEmergeSequence()
    {
        emergeSequence = DOTween.Sequence()
                    .Append(transform.DOMoveY(emergePoint.position.y, speed))
                    .SetSpeedBased(true)
                    .SetEase(Ease.OutSine);
    }

    private void StopEmergeSequence()
    {
        emergeSequence?.Kill();
        emergeSequence = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CompareLayers(waterLineLayer, collision.gameObject.layer))
        {
            StopEmergeSequence();
            PopTheBubble();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CustomerAir customerAir))
        {
            customerAir.IncreaseAir(airAmount);
            PopTheBubble();
        }
    }

    public void PopTheBubble()
    {
        OnBubblePopped?.Invoke(gameObject);
        StopEmergeSequence();
        Destroy(gameObject);
    }
}
