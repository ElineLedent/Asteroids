using UnityEngine;

/// <summary>
/// Base class for destroyer classes
/// </summary>
[RequireComponent(typeof(CollisionDetector))]
public abstract class ObjectDestroyer : MonoBehaviour
{
    [SerializeField]
    protected ObjectType m_ObjectType;

    [SerializeField]
    protected GameObject m_ExplosionPrefab;

    protected ScreenShaker m_ScreenShaker;

    protected bool m_ObjectCollided = false;

    protected void OnEnable()
    {
        m_ScreenShaker = Camera.main.GetComponent<ScreenShaker>();

        enabled &= DebugUtilities.Verify(m_ExplosionPrefab != null, "Explosion prefab not assigned");
        enabled &= DebugUtilities.Verify(m_ScreenShaker != null, "ScreenShaker script not found");

        gameObject.GetComponent<CollisionDetector>().CollisionDetected += OnCollisionDetected;
    }

    protected void OnDisable()
    {
        gameObject.GetComponent<CollisionDetector>().CollisionDetected -= OnCollisionDetected;
    }

    // Destroy in update so that collision is measured by all objects involved
    protected void Update()
    {
        if (m_ObjectCollided)
        {
            // Check if game is paused
            if (!SceneManager.Instance.GamePaused)
            {
                OnObjectDestroyed();

                Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);

                ExplosionSoundManager.Instance.PlayExplosionSound(m_ObjectType, transform.position);

                m_ScreenShaker.ShakeCamera(m_ObjectType);

                Destroy(this.gameObject);
            }
        }
    }

    protected void OnCollisionDetected()
    {
        m_ObjectCollided = true;
    }

    protected abstract void OnObjectDestroyed();
}