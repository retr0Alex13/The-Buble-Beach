using System;
using UnityEngine;

public class CustomerAir : MonoBehaviour
{
    [SerializeField] private float air;
    [SerializeField] private float maxAir = 100f;
    [SerializeField] private float airDecreaseRate = 0.1f;

    private float previousAirPercentage = -1f;

    private CustomerSwimming swimmingComponent;
    private CustomerColorChanger colorChangerComponent;

    private void Awake()
    {
        swimmingComponent = GetComponent<CustomerSwimming>();
        colorChangerComponent = GetComponent<CustomerColorChanger>();

        CustomerStayTime.OnCustomerLeave += RefillAir;
    }

    private void OnDestroy()
    {
        CustomerStayTime.OnCustomerLeave -= RefillAir;
    }

    private void Start()
    {
        air = maxAir;
    }

    private void Update()
    {
        // If drowned, return

        if (!swimmingComponent.IsInWater)
        {
            previousAirPercentage = -1f;
            return;
        }

        if (air == 0)
            return;

        if (air <= 0)
        {
            air = 0;
            // Customer drowned logic
            Debug.Log("Customer drowned");
            return;
        }

        DecreaseAirOverTime();
    }

    private void DecreaseAirOverTime()
    {
        air -= airDecreaseRate * Time.deltaTime;
        float currentAirPercentage = air / maxAir;

        if (Mathf.Abs(currentAirPercentage - previousAirPercentage) > 0.01f)
        {
            colorChangerComponent.UpdateColor(air, maxAir);
            previousAirPercentage = currentAirPercentage;
        }
    }

    public void IncreaseAir(float amount)
    {
        air += amount;
        if (air > maxAir)
            air = maxAir;
        SyncAirColor();
    }

    private void RefillAir()
    {
        air = maxAir;
        SyncAirColor();
    }

    private void SyncAirColor()
    {
        colorChangerComponent.UpdateColor(air, maxAir);
        previousAirPercentage = air / maxAir;
    }
}
