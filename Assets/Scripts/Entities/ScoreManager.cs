using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int pointsPerCustomer = 50;
    [SerializeField] private AudioClip scoreSound;

    private AudioSource audioSource;

    private int score = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddPoints()
    {
        audioSource.PlayOneShot(scoreSound);
        score += pointsPerCustomer;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }
}
