using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ValueType { SCORE, LIVES, LEVEL };

public enum PopUpType { EXIT_TO_MAIN, TOP_SCORES, RETRY_SCREEN };

public class UIGame : MonoSingleton<UIGame>
{
    [SerializeField]
    private GameObject m_UIPopUpExitToMain;

    [SerializeField]
    private GameObject m_UIPopUpTopScore;

    [SerializeField]
    private GameObject m_UIPopUpRetry;

    [SerializeField]
    private UIPopUpPoints m_UIPopUpPoints;

    [SerializeField]
    private UILifeCountDisplay m_UILifeCountDisplay;

    [SerializeField]
    private Text m_UITextScore;

    [SerializeField]
    private Text m_UITextLevel;

    [SerializeField]
    private Button m_UIButtonMainMenu;

    private Dictionary<PopUpType, GameObject> m_UIPopUps = new Dictionary<PopUpType, GameObject>();

    protected UIGame()
    {
    }

    private void OnEnable()
    {
        enabled &= DebugUtilities.Verify(m_UIPopUpExitToMain != null, "UI exit to main pop-up not assigned");
        enabled &= DebugUtilities.Verify(m_UIPopUpTopScore != null, "UI top score pop-up not assigned");
        enabled &= DebugUtilities.Verify(m_UIPopUpRetry != null, "UI retry pop-up not assigned");
        enabled &= DebugUtilities.Verify(m_UILifeCountDisplay != null, "UILifeCountDisplay script not assigned");
        enabled &= DebugUtilities.Verify(m_UIPopUpPoints != null, "UIPointsPopUp script not assigned");
        enabled &= DebugUtilities.Verify(m_UITextScore != null, "UI score text not assigned");
        enabled &= DebugUtilities.Verify(m_UITextLevel != null, "UI level text not assigned");
        enabled &= DebugUtilities.Verify(m_UIButtonMainMenu != null, "UI main menu button not assigned");

        GameStatistics.Instance.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        if (GameStatistics.Instance != null)
        {
            GameStatistics.Instance.ValueChanged -= OnValueChanged;
        }
    }

    private void Start()
    {
        m_UIPopUps[PopUpType.EXIT_TO_MAIN] = m_UIPopUpExitToMain;
        m_UIPopUps[PopUpType.TOP_SCORES] = m_UIPopUpTopScore;
        m_UIPopUps[PopUpType.RETRY_SCREEN] = m_UIPopUpRetry;

        HideAllPopUps();

        // Initialize GameUI values
        OnValueChanged(ValueType.LEVEL, GameStatistics.Instance.LevelNumber);
        OnValueChanged(ValueType.SCORE, GameStatistics.Instance.Score);
        OnValueChanged(ValueType.LIVES, GameStatistics.Instance.PlayerLives);
    }

    private void OnValueChanged(ValueType type, int value)
    {
        if (!enabled) { return; }

        switch (type)
        {
            case ValueType.SCORE:
                m_UITextScore.text = "Score: " + value;
                break;

            case ValueType.LIVES:
                m_UILifeCountDisplay.UpdateLivesDisplay();
                break;

            case ValueType.LEVEL:
                m_UITextLevel.text = "LEVEL " + value;
                break;

            default:
                Debug.Log("Invalid object type");
                break;
        }
    }

    public void DisplayLastEarnedPoints(int lastEarnedPoints, Vector3 lastDestroyedPosition)
    {
        if (!enabled) { return; }

        m_UIPopUpPoints.DisplayPoints(lastEarnedPoints, lastDestroyedPosition);
    }

    public void DisplayExitToMainMenu()
    {
        //Show exit to main pop up
        ShowPopUp(PopUpType.EXIT_TO_MAIN);
    }

    public void DisplayTopScore(int scoreRank)
    {
        UIPopUpTopScore topScoresPopUp = m_UIPopUps[PopUpType.TOP_SCORES].GetComponent<UIPopUpTopScore>();
        if (DebugUtilities.Verify(topScoresPopUp != null, "TopScoresPopUp script not found"))
        {
            topScoresPopUp.ScoreRank = scoreRank;
        }

        // Show top scores pop up
        ShowPopUp(PopUpType.TOP_SCORES);
    }

    public void DisplayRetryScreen()
    {
        ShowPopUp(PopUpType.RETRY_SCREEN);
    }

    public void ReturnToGame()
    {
        if (!enabled) { return; }

        // Unpause Game
        SceneManager.Instance.GamePaused = false;

        // Reenable main menu button
        m_UIButtonMainMenu.interactable = true;

        // Hide exit to main pop up
        HideAllPopUps();
    }

    private void ShowPopUp(PopUpType type)
    {
        if (!enabled) { return; }

        // Pause game
        SceneManager.Instance.GamePaused = true;

        // Make main menu button non-interactable
        m_UIButtonMainMenu.interactable = false;

        foreach (KeyValuePair<PopUpType, GameObject> popUp in m_UIPopUps)
        {
            popUp.Value.SetActive(popUp.Key == type);
        }
    }

    private void HideAllPopUps()
    {
        foreach (KeyValuePair<PopUpType, GameObject> popUp in m_UIPopUps)
        {
            popUp.Value.SetActive(false);
        }
    }
}