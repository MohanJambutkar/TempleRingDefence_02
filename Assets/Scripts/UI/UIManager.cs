using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gamePanel;
    public GameObject endPanel;

    [Header("Game UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI planetHealthText;
    public TextMeshProUGUI planetLivesText;

    [Header("End Screen")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI timeText;

    public ScoreManager scoreManager;
    public SolarEnergySystem energySystem;

    [Header("Managers")]
    public GameStateManager gameStateManager;
    public PlanetHealth planetHealth;

    void Update()
    {
        if (!gamePanel.activeSelf)
            return;

        // Listen for ESC key to pause game
        if (Input.GetKeyDown(KeyCode.Escape))
{
    if (gameStateManager != null)
    {
        gameStateManager.PauseGame();
    }
}

        scoreText.text = $"Score: {scoreManager.TotalScore:F1}";
        energyText.text = $"Energy: {energySystem.CurrentEnergy:F0}";
        if (planetHealth != null)
        {
            if (planetHealthText != null)
                planetHealthText.text = $"HP: {planetHealth.currentHP:F0}";

            if (planetLivesText != null)
                planetLivesText.text = $"Lives: {planetHealth.currentLives}";
        }
    }

    public void ShowStart() => SetOnly(startPanel);
    public void ShowGame()  => SetOnly(gamePanel);
    public void ShowPause() => SetOnly(pausePanel);

    public void ShowEnd(float score, string time)
    {
        SetOnly(endPanel);
        finalScoreText.text = $"Score: {score:F1}";
        timeText.text = $"Time: {time}";
    }

    void SetOnly(GameObject target)
    {
        startPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        endPanel.SetActive(false);
        target.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}