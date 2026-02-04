using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score")]
    [SerializeField] float totalScore;

    [Header("Survival Scoring")]
    [Tooltip("Score added per second survived (can be decimal)")]
    public float scorePerSecond = 1f;

    [Header("Survival Time (Read Only)")]
    [SerializeField] float survivedSeconds;
    [SerializeField] string survivedTimeFormatted;

    bool isRunning = true;

    public float TotalScore => totalScore;
    public float SurvivedSeconds => survivedSeconds;
    public string SurvivedTimeFormatted => survivedTimeFormatted;

    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (!isRunning)
            return;

        survivedSeconds += Time.deltaTime;
        totalScore += scorePerSecond * Time.deltaTime;

        UpdateFormattedTime();
    }

    void UpdateFormattedTime()
    {
        int minutes = Mathf.FloorToInt(survivedSeconds / 60f);
        int seconds = Mathf.FloorToInt(survivedSeconds % 60f);
        survivedTimeFormatted = $"{minutes:00}:{seconds:00}";
    }

    public void AddEnemyKillScore(int amount)
    {
        totalScore += amount;
    }

    /// <summary>
    /// Call this ONCE when the planet is destroyed
    /// </summary>
    public void StopScoring()
    {
        isRunning = false;
        UpdateFormattedTime();
    }
}