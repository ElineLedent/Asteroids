using UnityEngine;

/// <summary>
/// Wraps Objects around the screen
/// </summary>
public class ScreenWrapper : MonoBehaviour
{
    public delegate void ScreenWrapEventHandler();

    public event ScreenWrapEventHandler ScreenWrap;

    private Renderer m_Renderer;

    private void OnEnable()
    {
        m_Renderer = gameObject.GetComponentInChildren<Renderer>();
        enabled &= DebugUtilities.Verify(m_Renderer != null, "Renderer not found");
    }

    private void Update()
    {
        if (!SceneManager.Instance.GamePaused)
        {
            // Recalculate position
            if (Camera.main != null && !m_Renderer.isVisible)
            {
                OnScreenWrap();

                Vector3 targetPosition = transform.position;

                Vector3 leftBottomScreenEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
                Vector3 leftBottomMinBounds = m_Renderer.bounds.min;
                Vector3 rightTopMaxBounds = m_Renderer.bounds.max;

                // Calculate target position at the other side of the screen
                if (rightTopMaxBounds.x < leftBottomScreenEdge.x)
                {
                    // Wrap around screen
                    targetPosition.x = -transform.position.x;

                    float difference = targetPosition.x * 2;

                    targetPosition.x = targetPosition.x - (leftBottomMinBounds.x + difference - (-leftBottomScreenEdge.x));
                }
                else if (leftBottomMinBounds.x > -leftBottomScreenEdge.x)
                {
                    targetPosition.x = -transform.position.x;

                    float difference = -targetPosition.x * 2;

                    targetPosition.x = targetPosition.x - (rightTopMaxBounds.x - difference - (leftBottomScreenEdge.x));
                }

                if (rightTopMaxBounds.y < leftBottomScreenEdge.y)
                {
                    targetPosition.y = -transform.position.y;

                    float difference = targetPosition.y * 2;

                    targetPosition.y = targetPosition.y - (leftBottomMinBounds.y + difference - (-leftBottomScreenEdge.y));
                }
                else if (leftBottomMinBounds.y > -leftBottomScreenEdge.y)
                {
                    targetPosition.y = -transform.position.y;

                    float difference = -targetPosition.y * 2;

                    targetPosition.y = targetPosition.y - (rightTopMaxBounds.y - difference - (leftBottomScreenEdge.y));
                }

                // Move object to the target position
                transform.position = targetPosition;
            }
        }
    }

    private void OnScreenWrap()
    {
        if (ScreenWrap != null)
        {
            ScreenWrap();
        }
    }
}