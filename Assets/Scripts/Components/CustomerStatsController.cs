using UnityEngine;

public class CustomerStatsController : MonoBehaviour
{
    private float air = 100f;
    private float airDecreaseRate = 0.1f;
    private float visitingTime = 100f;
    private float maxVisitingTime = 200f;

    private CustomerBehaviour customerBehaviour;

    private void Awake()
    {
        customerBehaviour = GetComponent<CustomerBehaviour>();
    }

    private void Start()
    {
        visitingTime = Random.Range(visitingTime, maxVisitingTime);
    }

    private void Update()
    {
        UpdateCustomerStats();
    }

    private void UpdateCustomerStats()
    {
        if (customerBehaviour.IsInWater)
        {
            air -= airDecreaseRate * Time.deltaTime;
            if (air <= 0)
            {
                // Customer is dead
            }
        }
        else
        {
            visitingTime -= Time.deltaTime;
            if (visitingTime <= 0)
            {
                // Cutomer is leaving
            }
        }
    }
}
