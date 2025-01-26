using UnityEngine;

public class CustomerSwimming : MonoBehaviour
{
    public bool IsInWater { get; private set; }

    [SerializeField] private LayerMask waterLayer;

    [SerializeField] private float swimDelay = 2f;
    [SerializeField] private float maxSwimDelay = 5f;

    private BoxCollider2D swimmingZone;
    private AudioSource audioSource;
    private Animator animator;

    private MoveCustomer moveCustomer;
    private CustomerAir customersAir;

    private bool IsJumped;

    private float nextSwimTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CompareLayers(waterLayer, collision.gameObject.layer))
        {
            if (!IsJumped)
            {
                audioSource.Play();
                IsJumped = true;
            }
            IsInWater = true;
            animator.SetBool("Swim", true);
            nextSwimTime = Time.time + Random.Range(swimDelay, maxSwimDelay);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // NOTE: In project setting I have set customer
        // and destroy bubble line layers to ignore each other collision
        if (!Utils.CompareLayers(waterLayer, collision.gameObject.layer))
        {
            IsInWater = false;
            animator.SetBool("Swim", false);
        }
    }

    private void Awake()
    {
        moveCustomer = GetComponent<MoveCustomer>();
        customersAir = GetComponent<CustomerAir>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        swimmingZone = FindAnyObjectByType<SwimmingZone>().GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandleSwimming();
    }

    private void HandleSwimming()
    {
        if (customersAir.IsDrowned)
            return;

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