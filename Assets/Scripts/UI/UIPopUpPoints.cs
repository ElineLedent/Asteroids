using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIPopUpPoints : MonoBehaviour
{
    [SerializeField]
    private float m_PointsDisplayMaxTime = 1f;

    private Text m_PointsText;

    private float m_Time = 0f;

    private bool m_IsShowing = false;

    private void OnEnable()
    {
        m_PointsText = gameObject.GetComponent<Text>();
        m_PointsText.enabled = false;
    }

    private void Update()
    {
        if (m_IsShowing)
        {
            if (Time.time - m_Time >= m_PointsDisplayMaxTime)
            {
                m_PointsText.enabled = false;
                m_IsShowing = false;
            }
        }
    }

    public void DisplayPoints(int lastEarnedPoints, Vector3 lastDestroyedPosition)
    {
        m_PointsText.enabled = true;
        m_IsShowing = true;
        m_Time = Time.time;

        // Convert destroyed object position to screen point
        Vector3 objectScreenPosition = Camera.main.WorldToScreenPoint(lastDestroyedPosition);
        gameObject.transform.position = objectScreenPosition;
        m_PointsText.text = lastEarnedPoints.ToString();
    }
}