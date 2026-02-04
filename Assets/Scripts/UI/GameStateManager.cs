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
}