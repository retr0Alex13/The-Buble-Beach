using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class BubbleSpawnManager : MonoBehaviour
{
    [SerializeField] private int maxBubbles = 5;
    [SerializeField] private int bubblesToCustomerRatio = 2;

    [SerializeField] private float spawnDelay = 0.3f;
    [SerializeField] private float spawnRate = 10f;

    [Inject(Id = "BubblePrefab")] private GameObject bubblePrefab;
    [Inject] DiContainer container;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform bubblesParent;

    private List<GameObject> activeBubbles = new List<GameObject>();
    private CustomerSpawnManager customerManager;

    private void Awake()
    {
        customerManager = GetComponent<CustomerSpawnManager>();
    }

    private void Update()
    {
        int targetBubbleCount = customerManager.CustomersCount / bubblesToCustomerRatio;

        while (activeBubbles.Count < targetBubbleCount && activeBubbles.Count < maxBubbles)
        {
            StartCoroutine(SpawnBubblesCoroutine(targetBubbleCount - activeBubbles.Count));
        }
    }
    private IEnumerator SpawnBubblesCoroutine(int bubblesToSpawn)
    {
        // Цикл для створення бульбашок з затримкою
        for (int i = 0; i < bubblesToSpawn; i++)
        {
            SpawnBubble();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnBubble()
    {
        GameObject bubbleInstance = container.InstantiatePrefab
            (bubblePrefab, spawnPoint.position, Quaternion.identity, bubblesParent);

        activeBubbles.Add(bubbleInstance);

        if (bubbleInstance.TryGetComponent(out Bubble bubble))
        {
            bubble.OnBubblePopped += OnBubblePopped;
        }
    }

    private void OnBubblePopped(GameObject bubble)
    {
        if (activeBubbles.Contains(bubble))
        {
            activeBubbles.Remove(bubble);
        }
    }
}
