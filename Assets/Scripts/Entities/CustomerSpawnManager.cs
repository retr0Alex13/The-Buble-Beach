using System.Collections;
using UnityEngine;
using Zenject;

public class CustomerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [Inject(Id = "CustomerPrefab")] private GameObject customerPrefab;

    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float maxSpawnInterval = 10f;
    [SerializeField] private int maxCustomers = 5;

    private int currentCustomers;

    [Inject] private DiContainer container;

    private void Start()
    {
        StartCoroutine(SpawnVisitors());
    }

    private IEnumerator SpawnVisitors()
    {
        while (true)
        {
            if (currentCustomers < maxCustomers)
            {
                GameObject customerInstance = container.InstantiatePrefab(customerPrefab, spawnPoint.position, Quaternion.identity, null);
                currentCustomers++;
            }
            float randomSpawnInterval = Random.Range(spawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    public void CustomerLeft(GameObject customer)
    {
        currentCustomers--;
        Destroy(customer);
    }
}
