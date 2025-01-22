using UnityEngine;
using DG.Tweening;

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
    private MoveCustomer moveCustomer;
    private CustomerColorChanger customerColorChanger;

    private float previousAirPercentage = -1f; // Змінна для відстеження попереднього відсотка повітря

    private void Awake()
    {
        customerBehaviour = GetComponent<CustomerBehaviour>();
        customerColorChanger = GetComponent<CustomerColorChanger>();
        moveCustomer = GetComponent<MoveCustomer>();
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
        if (!customerBehaviour.IsInWater)
        {
            previousAirPercentage = -1f; // Скидаємо значення при виході з води
            return;
        }

        if (air <= 0) // Перевірка на нуль або менше
        {
            air = 0;
            // Customer is dead
            return;
        }

        if (visitingTime <= 0)
        {
            visitingTime = 0;
            moveCustomer.LeaveBeach();
            IncreaseAir(maxAir);
            return;
        }

        air -= airDecreaseRate * Time.deltaTime;
        visitingTime -= visitingTimeDecreaseRate * Time.deltaTime;

        float currentAirPercentage = air / maxAir;

        if (Mathf.Abs(currentAirPercentage - previousAirPercentage) > 0.01f)
        {
            customerColorChanger.UpdateColor(air, maxAir);
            previousAirPercentage = currentAirPercentage;
        }
    }

    private void IncreaseAir(float amount)
    {
        air += amount;
        if (air > maxAir)
            air = maxAir;

        customerColorChanger.UpdateColor(air, maxAir);
        previousAirPercentage = air / maxAir;
    }
}