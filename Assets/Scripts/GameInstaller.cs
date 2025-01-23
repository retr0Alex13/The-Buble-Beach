using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform drownLine;
    [SerializeField] private GameObject customerPrefab;

    [SerializeField] private Transform emergePoint;
    [SerializeField] private GameObject bubblePrefab;

    public override void InstallBindings()
    {
        BindCustomerPrefab();
        BindBubblePrefab();
    }

    private void BindBubblePrefab()
    {
        Container.Bind<Transform>().WithId("EmergePoint").FromInstance(emergePoint);
        Container.Bind<Bubble>().FromComponentInNewPrefab(bubblePrefab).AsSingle();
        Container.Bind<GameObject>().WithId("BubblePrefab").FromInstance(bubblePrefab);
    }

    private void BindCustomerPrefab()
    {
        Container.Bind<Transform>().WithId("JumpPoint").FromInstance(jumpPoint);
        Container.Bind<Transform>().WithId("StartPoint").FromInstance(startPoint);
        Container.Bind<Transform>().WithId("DrownLine").FromInstance(drownLine);

        Container.Bind<MoveCustomer>().FromComponentInNewPrefab(customerPrefab).AsTransient();
        Container.Bind<GameObject>().WithId("CustomerPrefab").FromInstance(customerPrefab);
    }
}