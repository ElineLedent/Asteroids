using UnityEngine;

public class SceneManager : MonoSingleton<SceneManager>
{
    public delegate void PauseStatusChangedEventHandler();

    public event PauseStatusChangedEventHandler PauseStatusChanged;

    private bool m_GamePaused = true;

    public bool GamePaused
    {
        set
        {
            if (m_GamePaused != value)
            {
                m_GamePaused = value;
                OnPauseStatusChanged();
            }
            Time.timeScale = m_GamePaused ? 0f : 1f;
        }

        get { return m_GamePaused; }
    }

    protected SceneManager()
    {
    }

    private void Start()
    {
        LoadMainMenu();
    }

    private void OnPauseStatusChanged()
    {
        if (PauseStatusChanged != null)
        {
            PauseStatusChanged();
        }
    }

    public void LoadMainMenu()
    {
        GameStatistics.Instance.ResetGameStatistics();

        UnityEngine.SceneManagement.SceneManager.LoadScene("UIMainMenu");
    }

    public void LoadNewGame()
    {
        GameStatistics.Instance.ResetGameStatistics();

        LoadGameLevel();
    }

    public void LoadGameLevel()
    {
        GamePaused = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene("AsteroidsLevel");

        UnityEngine.SceneManagement.SceneManager.LoadScene("UIGame", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}