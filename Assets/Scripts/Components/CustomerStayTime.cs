using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class CustomerStayTime : MonoBehaviour
{
    public static event Action OnCustomerLeave;

    [SerializeField] private float visitingTime;
    [SerializeField] private float minVisitingTime = 100f;
    [SerializeField] private float maxVisitingTime = 200f;
    [SerializeField] private float visitingTimeDecreaseRate = 4f;

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
        // if drowned, return

        if (visitingTime == 0)
        {
            return;
        }

        if (visitingTime <= 0)
        {
            visitingTime = 0;
            OnCustomerLeave?.Invoke();
            return;
        }

        visitingTime -= visitingTimeDecreaseRate * Time.deltaTime;
    }
}