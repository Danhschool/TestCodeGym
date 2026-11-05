using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score")]
    [SerializeField] private int targetScore = 10;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string scoreFormat = "Score: {0}";

    [Header("Win")]
    [SerializeField] private TMP_Text winText;
    [SerializeField] private string winMessage = "You Win!";

    [Header("References")]
    [SerializeField] private ItemSpawner itemSpawner;

    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (itemSpawner == null)
        {
            itemSpawner = FindAnyObjectByType<ItemSpawner>();
        }
    }

    private void Start()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
        UpdateScoreText();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void AddScore(int amount)
    {
        if (IsGameOver) return;

        Score += amount;
        UpdateScoreText();

        if (Score >= targetScore)
        {
            Win();
        }
    }

    private void Win()
    {
        IsGameOver = true;

        if (itemSpawner != null)
        {
            itemSpawner.StopSpawning();
        }

        if (winText != null)
        {
            winText.text = winMessage;
            winText.gameObject.SetActive(true);
        }

        Debug.Log(winMessage);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = string.Format(scoreFormat, Score);
        }
    }
}
