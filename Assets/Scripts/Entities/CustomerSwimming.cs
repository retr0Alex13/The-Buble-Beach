using UnityEngine;

public class CustomerSwimming : MonoBehaviour
{
    public bool IsInWater { get; private set; }

    [SerializeField] private LayerMask waterLayer;

    [SerializeField] private float swimDelay = 2f;
    [SerializeField] private float maxSwimDelay = 5f;

    private BoxCollider2D swimmingZone;
    private MoveCustomer moveCustomer;
    private float nextSwimTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CompareLayers(waterLayer, collision.gameObject.layer))
        {
            IsInWater = true;
            nextSwimTime = Time.time + Random.Range(swimDelay, maxSwimDelay);
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

    private void Awake()
    {
        moveCustomer = GetComponent<MoveCustomer>();
    }

    private void Update()
    {
        if (!IsInWater || moveCustomer.IsMoving)
            return;

        if (Time.time >= nextSwimTime)
        {
            StartMoving();
            nextSwimTime = Time.time + Random.Range(swimDelay, maxSwimDelay);
        }
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