using UnityEngine;
using Zenject;

public class CustomerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [Inject(Id = "CustomerPrefab")] private GameObject customerPrefab;

    [Inject] private DiContainer container;

    private void Start()
    {
        GameObject customerInstance = container.InstantiatePrefab(customerPrefab, spawnPoint.position, Quaternion.identity, null);
        MoveCustomer moveCustomer = customerInstance.GetComponent<MoveCustomer>();
    }
}
