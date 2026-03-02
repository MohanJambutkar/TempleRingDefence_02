using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool IsPaused { get; private set; }

    [Header("UI")]
    public UIManager ui;

    void Awake()
    {
        Instance = this;

        // Start game in paused state until Play is pressed
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        ui.ShowGame();
    }

    public void PauseGame()
    {
        if (IsPaused)
            ResumeGame();
        else
            DoPause();
    }

    void DoPause()
    {
        Time.timeScale = 0f;
        IsPaused = true;
        ui.ShowPause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        ui.ShowGame();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void GameOver()
{
    Time.timeScale = 0f;
    IsPaused = true;

    if (ui != null)
        ui.ShowEnd(
            ScoreManager.Instance.TotalScore,
            ScoreManager.Instance.SurvivedTimeFormatted
        );
}
}