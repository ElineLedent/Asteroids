using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CollisionDetector))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float m_BulletSpeed = 20f;

    private Vector3 m_ProjectileDirection = Vector3.zero;
    private Vector3 m_StartVelocity = Vector3.zero;
    private float m_BulletLifeTime = 0f;
    private bool m_ObjectCollided = false;

    public Vector3 StartVelocity
    {
        set { m_StartVelocity = value; }
    }

    public Vector3 ProjectileDirection
    {
        set { m_ProjectileDirection = value; }
    }

    private void OnEnable()
    {
        m_ObjectCollided = false;

        // Set direction and speed of bullet
        gameObject.GetComponent<Rigidbody2D>().velocity = m_StartVelocity + (m_BulletSpeed * m_ProjectileDirection);

        // Calculate width of screen in world units
        float screenWidth = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x - Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        // Calculate lifetime of bullet through the formula time = distance/speed, where distance is half of screen width
        m_BulletLifeTime = (screenWidth / 2) / gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        // Invoke automatic self destruction after a distance of half the screen
        Invoke("DestroyProjectile", m_BulletLifeTime);

        gameObject.GetComponent<CollisionDetector>().CollisionDetected += OnCollisionDetected;
    }

    private void OnDisable()
    {
        CancelInvoke();

        gameObject.GetComponent<CollisionDetector>().CollisionDetected -= OnCollisionDetected;
    }

    // Destroy in update so that collision is measured by all objects involved
    private void Update()
    {
        if (m_ObjectCollided)
        {
            // Check if game is paused
            if (!SceneManager.Instance.GamePaused)
            {
                DestroyProjectile();
            }
        }
    }

    private void OnCollisionDetected()
    {
        m_ObjectCollided = true;
    }

    private void DestroyProjectile()
    {
        gameObject.SetActive(false);
    }
}