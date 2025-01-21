using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private BoxCollider2D SwimmingZone;

    [SerializeField] private float swimDelay = 2f;
    [SerializeField] private float maxSwimDelay = 5f;

    private bool isSwimming;

    private MoveCustomer moveCustomer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CompareLayers(waterLayer, collision.gameObject.layer))
        {
            isSwimming = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Utils.CompareLayers(waterLayer, collision.gameObject.layer))
        {
            isSwimming = false;
        }
    }

    private void Awake()
    {
        moveCustomer = GetComponent<MoveCustomer>();
    }

    private void Update()
    {
        if (!isSwimming || moveCustomer.IsMoving)
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
        float randomX = Random.Range(SwimmingZone.bounds.min.x, SwimmingZone.bounds.max.x);
        float randomY = Random.Range(SwimmingZone.bounds.min.y, SwimmingZone.bounds.max.y);
        Vector2 randomPosition = new Vector3(randomX, randomY);

        return randomPosition;
    }
}
