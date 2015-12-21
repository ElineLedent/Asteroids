using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class UILifeCountDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject m_UIPlayerShip;

    private void OnEnable()
    {
        enabled &= DebugUtilities.Verify(m_UIPlayerShip != null, "UI player ship image not assigned");
    }

    public void UpdateLivesDisplay()
    {
        if (!enabled) { return; }

        foreach (Transform displayedLife in gameObject.transform)
        {
            GameObject.Destroy(displayedLife.gameObject);
        }

        for (int i = 0; i < GameStatistics.Instance.PlayerLives; ++i)
        {
            GameObject playerLivesUI = (GameObject)Instantiate(m_UIPlayerShip);
            playerLivesUI.transform.SetParent(gameObject.transform, false);
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        ResizeLivesDisplay();
    }

    private void ResizeLivesDisplay()
    {
        int ratio = 15;
        gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.height / ratio, Screen.height / ratio);
    }
}