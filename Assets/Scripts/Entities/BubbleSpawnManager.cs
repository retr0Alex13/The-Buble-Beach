using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BubbleSpawnManager : MonoBehaviour
{
    [SerializeField] private int maxBubbles = 5;
    [SerializeField] private int bubblesToCustomerRatio = 2;

    [SerializeField] private float spawnDelay = 0.3f;

    [Inject(Id = "BubblePrefab")] private GameObject bubblePrefab;
    [Inject] DiContainer container;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform bubblesParent;

    [SerializeField] private AudioClip[] bubblePopSounds;
    private AudioSource audioSource;

    private List<GameObject> activeBubbles = new List<GameObject>();
    private CustomerSpawnManager customerManager;

    private void Awake()
    {
        customerManager = GetComponent<CustomerSpawnManager>();
        audioSource = GetComponent<AudioSource>();
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
        // ���� ��� ��������� ��������� � ���������
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
            if (!audioSource.isPlaying)
            {
                int randomIndex = Random.Range(0, bubblePopSounds.Length);
                audioSource.PlayOneShot(bubblePopSounds[randomIndex]);
            }
            activeBubbles.Remove(bubble);
        }
    }
}
