using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class CustomerStayTime : MonoBehaviour
{
    public event Action OnCustomerLeave;
    public float VisitingTime => visitingTime;

    [SerializeField] private float visitingTime;
    [SerializeField] private float minVisitingTime = 100f;
    [SerializeField] private float maxVisitingTime = 200f;
    [SerializeField] private float visitingTimeDecreaseRate = 4f;

    private CustomerAir customerAir;
    private bool hasLeft;

    private void Awake()
    {
        customerAir = GetComponent<CustomerAir>();
    }

    private void Start()
    {
        visitingTime = Random.Range(minVisitingTime, maxVisitingTime);
    }

    private void Update()
    {
        DecreaseStayDuration();
    }

    private void DecreaseStayDuration()
    {

        if (customerAir.IsDrowned)
            return;

        if (visitingTime <= 0 && !hasLeft)
        {
            visitingTime = 0;
            OnCustomerLeave?.Invoke();
            hasLeft = true;
            return;
        }

        if (visitingTime > 0)
        {
            visitingTime -= visitingTimeDecreaseRate * Time.deltaTime;
        }
    }
}