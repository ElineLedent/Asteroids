using UnityEngine;
using UnityEngine.UI;

public class UIPopUpTopScore : MonoBehaviour
{
    [SerializeField]
    private Button m_UIOkButton;

    [SerializeField]
    private Text m_UITopScorePopUpText;

    private int m_UIScoreRank = 0;

    public int ScoreRank
    {
        set
        {
            m_UIScoreRank = value;
            if (enabled)
            {
                m_UITopScorePopUpText.text = "CONGRATULATIONS!\nNEW TOP SCORE: " + GameStatistics.Instance.Score + "   RANK: " + m_UIScoreRank;
            }
        }
    }

    private void OnEnable()
    {
        enabled &= DebugUtilities.Verify(m_UIOkButton != null, "Ok button not assigned");
        enabled &= DebugUtilities.Verify(m_UITopScorePopUpText != null, "Top score pop up text not assigned");
    }

    private void Start()
    {
        // Give functionality to button
        m_UIOkButton.onClick.AddListener(delegate { UIGame.Instance.DisplayRetryScreen(); });
    }
}