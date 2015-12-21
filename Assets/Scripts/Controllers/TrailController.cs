using UnityEngine;

[RequireComponent(typeof(ScreenWrapper))]
public class TrailController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_TrailSocketLeft;

    [SerializeField]
    private GameObject m_TrailSocketRight;

    [SerializeField]
    private GameObject m_TrailPrefab;

    private TrailRenderer m_TrailRendererLeft;

    private TrailRenderer m_TrailRendererRight;

    private void OnEnable()
    {
        enabled &= DebugUtilities.Verify(m_TrailSocketLeft != null, "Fire trail socket not assigned");
        enabled &= DebugUtilities.Verify(m_TrailSocketRight != null, "Fire trail socket not assigned");
        enabled &= DebugUtilities.Verify(m_TrailPrefab != null, "Trail prefab not assigned");

        if (enabled)
        {
            m_TrailRendererLeft = CreateNewTrailRenderer(m_TrailSocketLeft);
            m_TrailRendererRight = CreateNewTrailRenderer(m_TrailSocketRight);
        }
        enabled &= DebugUtilities.Verify(m_TrailRendererLeft != null, "Failed to create trail renderer");
        enabled &= DebugUtilities.Verify(m_TrailRendererRight != null, "Failed to create trail renderer");

        ScreenWrapper screenWrapper = gameObject.GetComponent<ScreenWrapper>();
        screenWrapper.ScreenWrap += OnScreenWrap;
    }

    private void OnDisable()
    {
        ScreenWrapper screenWrapper = gameObject.GetComponent<ScreenWrapper>();
        screenWrapper.ScreenWrap -= OnScreenWrap;
    }

    private void OnScreenWrap()
    {
        if (!enabled) { return; }

        //Destroy old trail renderers
        DestroyImmediate(m_TrailRendererLeft);
        DestroyImmediate(m_TrailRendererRight);

        //Create new trail renderers
        m_TrailRendererLeft = CreateNewTrailRenderer(m_TrailSocketLeft);
        m_TrailRendererRight = CreateNewTrailRenderer(m_TrailSocketRight);
    }

    private TrailRenderer CreateNewTrailRenderer(GameObject trailSocket)
    {
        GameObject trailRendererObject = (GameObject)Instantiate(m_TrailPrefab);

        trailRendererObject.transform.SetParent(trailSocket.transform);
        trailRendererObject.transform.localPosition = Vector3.zero;

        TrailRenderer trailRenderer = trailRendererObject.GetComponent<TrailRenderer>();

        return trailRenderer;
    }
}