using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private Transform startPoint;

    [SerializeField] private GameObject customerPrefab;

    public override void InstallBindings()
    {
        BindCustomerPrefab();
    }

    private void BindCustomerPrefab()
    {
        Container.Bind<Transform>().WithId("JumpPoint").FromInstance(jumpPoint);
        Container.Bind<Transform>().WithId("StartPoint").FromInstance(startPoint);

        Container.Bind<MoveCustomer>().FromComponentInNewPrefab(customerPrefab).AsTransient();

        Container.Bind<GameObject>().WithId("CustomerPrefab").FromInstance(customerPrefab);
    }
}