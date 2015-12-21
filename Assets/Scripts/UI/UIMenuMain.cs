using UnityEngine;

public class UIMenuMain : MonoBehaviour
{
    private UIMenuController m_UIMenuController;

    private void OnEnable()
    {
        m_UIMenuController = gameObject.GetComponentInParent<UIMenuController>();

        enabled &= DebugUtilities.Verify(m_UIMenuController != null, "MenuController script not found");
    }

    public void StartGame()
    {
        SceneManager.Instance.LoadGameLevel();
    }

    public void ShowTopScores()
    {
        if (!enabled) { return; }

        m_UIMenuController.ShowMenu(MenuType.TOPSCORES);
    }

    public void ShowGameHistory()
    {
        if (!enabled) { return; }

        m_UIMenuController.ShowMenu(MenuType.HISTORY);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}