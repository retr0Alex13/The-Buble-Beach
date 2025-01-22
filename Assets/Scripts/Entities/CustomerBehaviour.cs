using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    public bool IsInWater { get; private set; }

    [SerializeField] private LayerMask waterLayer;

    [SerializeField] private float swimDelay = 2f;
    [SerializeField] private float maxSwimDelay = 5f;

    private BoxCollider2D swimmingZone;
    private MoveCustomer moveCustomer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CompareLayers(waterLayer, collision.gameObject.layer))
        {
            IsInWater = true;
        }
        if (collision.TryGetComponent(out SwimmingZone swimmingZoneComponent))
        {
            swimmingZone = collision.GetComponent<BoxCollider2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!Utils.CompareLayers(waterLayer, collision.gameObject.layer))
        {
            IsInWater = false;
        }
    }

    private void HandleWaterCollision(Collider2D collision)
    {
        IsInWater = Utils.CompareLayers(waterLayer, collision.gameObject.layer) ? true : false;
    }

    private void Awake()
    {
        moveCustomer = GetComponent<MoveCustomer>();
    }

    private void Update()
    {
        Debug.Log($"Active Tweens: {DOTween.TotalPlayingTweens()}");

        if (!IsInWater || moveCustomer.IsMoving)
            return;

        float randomDelay = Random.Range(swimDelay, maxSwimDelay);
        Invoke(nameof(StartMoving), randomDelay);
    }
    private void StartMoving()
        {
            moveCustomer.MoveToPosition(GetRandomSwimPosition());
        }

    private Vector2 GetRandomSwimPosition()
    {
        float randomX = Random.Range(swimmingZone.bounds.min.x, swimmingZone.bounds.max.x);
        float randomY = Random.Range(swimmingZone.bounds.min.y, swimmingZone.bounds.max.y);
        Vector2 randomPosition = new Vector3(randomX, randomY);

        return randomPosition;
    }
}
