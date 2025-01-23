using UnityEngine;
using Zenject;

public class BubbleSpawnManager : MonoBehaviour
{
    [SerializeField] private float spawnRate = 10f;
    [Inject(Id = "BubblePrefab")] private GameObject bubblePrefab;
    [SerializeField] private Transform spawnPoint;

    [Inject] DiContainer container;


    void Start()
    {
        InvokeRepeating(nameof(SpawnBubble), 0f, spawnRate);
    }

    private void SpawnBubble()
    {
        GameObject bubbleInstance = container.InstantiatePrefab(bubblePrefab, spawnPoint.position, Quaternion.identity, null);
        Debug.Log("Bubble spawned");
    }
}
