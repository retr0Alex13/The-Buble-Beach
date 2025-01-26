using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CustomerSpawnManager : MonoBehaviour
{
    public int CustomersCount => currentCustomersCount;

    [SerializeField] private Transform spawnPoint;
    [Inject(Id = "CustomerPrefab")] private GameObject customerPrefab;

    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float maxSpawnInterval = 10f;
    [SerializeField] private int maxCustomers = 5;
    [SerializeField] private Transform customersParent;
    private int currentCustomersCount;

    [Inject] private DiContainer container;

    private void Start()
    {
        StartCoroutine(SpawnVisitors());
    }

    private IEnumerator SpawnVisitors()
    {
        while (true)
        {
            if (currentCustomersCount < maxCustomers)
            {
                GameObject customerInstance = container.InstantiatePrefab(customerPrefab, spawnPoint.position,
                    Quaternion.identity, customersParent);
                currentCustomersCount++;
            }
            float randomSpawnInterval = Random.Range(spawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    public void CustomerLeft(GameObject customer)
    {
        if (customer != null)
        {
            currentCustomersCount--;
            //Destroy(customer);
        }
    }
}
