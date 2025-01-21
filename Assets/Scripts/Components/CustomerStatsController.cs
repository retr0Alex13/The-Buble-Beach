using UnityEngine;

public class CustomerStatsController : MonoBehaviour
{
    [Header("Air settings")]
    [SerializeField] private float air;
    [SerializeField] private float maxAir = 100f;
    [SerializeField] private float airDecreaseRate = 0.1f;

    [Space(10), Header("Visiting Time")]
    [SerializeField] private float visitingTime;
    [SerializeField] private float minVisitingTime = 100f;
    [SerializeField] private float maxVisitingTime = 200f;
    [SerializeField] private float visitingTimeDecreaseRate = 0.1f;

    private CustomerBehaviour customerBehaviour;
    private CustomerColorChanger customerColorChanger;

    private void Awake()
    {
        customerBehaviour = GetComponent<CustomerBehaviour>();
        customerColorChanger = GetComponent<CustomerColorChanger>();
    }

    private void Start()
    {
        air = maxAir;
        visitingTime = Random.Range(minVisitingTime, maxVisitingTime);
    }

    private void Update()
    {
        UpdateCustomerStats();
    }

    private void UpdateCustomerStats()
    {
        if (customerBehaviour.IsInWater)
        {
            if (air <= 0)
            {
                air = 0;
                // Customer is dead
                return;
            }
            else
            {
                air -= airDecreaseRate * Time.deltaTime;
                customerColorChanger.UpdateColor(air, maxAir);
                Debug.Log($"Повітря відвідувача {gameObject.name}: {air}");
            }

            if (visitingTime <= 0)
            {
                visitingTime = 0;
                // Cutomer is leaving
                return;
            }
            else
            {
                visitingTime -= visitingTimeDecreaseRate * Time.deltaTime;
                Debug.Log($"Час відвідування {gameObject.name}: {air}");
            }
        }
    }
}
