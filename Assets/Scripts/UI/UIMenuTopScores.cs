using UnityEngine;
using UnityEngine.UI;

public class UIMenuTopScores : MonoBehaviour
{
    [SerializeField]
    private Text[] m_UITopScores;

    private void OnEnable()
    {
        for (int i = 0; i < m_UITopScores.Length; ++i)
        {
            enabled &= DebugUtilities.Verify(m_UITopScores[i] != null, "Top score " + i + " text not assigned");
        }

        enabled &= DebugUtilities.Verify(m_UITopScores.Length == TopScoreManager.TOTAL_TOPSCORES, "Not enough text fields assigned to array");
    }

    private void Start()
    {
        for (int i = 0; i < m_UITopScores.Length; ++i)
        {
            m_UITopScores[i].text = TopScoreManager.Instance.GetTopScore(i).ToString();
        }
    }

    public void ReturnToMainMenu()
    {
        UIMenuController menuController = gameObject.GetComponentInParent<UIMenuController>();
        if (DebugUtilities.Verify(menuController != null, "MenuController script not found"))
        {
            menuController.ShowMenu(MenuType.MAIN);
        }
    }
}