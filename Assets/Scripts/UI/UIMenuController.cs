using System.Collections.Generic;
using UnityEngine;

public enum MenuType { MAIN, TOPSCORES, HISTORY };

public class UIMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_UIMenuMain;

    [SerializeField]
    private GameObject m_UIMenuTopScore;

    [SerializeField]
    private GameObject m_UIMenuGameHistory;

    private Dictionary<MenuType, GameObject> m_UIMenus = new Dictionary<MenuType, GameObject>();

    private void OnEnable()
    {
        enabled &= DebugUtilities.Verify(m_UIMenuMain != null, "Main menu object not assigned");
        enabled &= DebugUtilities.Verify(m_UIMenuTopScore != null, "Top scores menu object not assigned");
        enabled &= DebugUtilities.Verify(m_UIMenuGameHistory != null, "Game history menu object not assigned");
    }

    private void Start()
    {
        m_UIMenus[MenuType.MAIN] = m_UIMenuMain;
        m_UIMenus[MenuType.TOPSCORES] = m_UIMenuTopScore;
        m_UIMenus[MenuType.HISTORY] = m_UIMenuGameHistory;

        ShowMenu(MenuType.MAIN);
    }

    public void ShowMenu(MenuType type)
    {
        if (!enabled) { return; }

        foreach (KeyValuePair<MenuType, GameObject> menu in m_UIMenus)
        {
            menu.Value.SetActive(menu.Key == type);
        }
    }
}