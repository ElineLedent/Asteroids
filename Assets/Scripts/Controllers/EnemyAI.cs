using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private ObjectType m_EnemyType;

    [SerializeField]
    private GameObject m_Muzzle1;

    [SerializeField]
    private GameObject m_Muzzle2;

    [SerializeField]
    private float m_EnemySpeed = 10f;

    [SerializeField]
    private int m_RotationDice = 100;

    [SerializeField]
    private int m_TurnToPlayerDice = 200;

    [SerializeField]
    private int m_EnemyFireDice = 30;

    private AudioSource m_EngineAudioSource;

    private FireController m_FireController = new FireController();

    public ObjectType EnemyType
    {
        get { return m_EnemyType; }
    }

    private void OnEnable()
    {
        enabled &= DebugUtilities.Verify(m_Muzzle1 != null, "Enemy muzzle one child object not assigned");
        enabled &= DebugUtilities.Verify(m_Muzzle2 != null, "Enemy muzzle two child object not assigned");

        m_EngineAudioSource = gameObject.GetComponent<AudioSource>();

        RotateEnemy(Random.Range(0, 360));

        SceneManager.Instance.PauseStatusChanged += OnPauseStatusChanged;
    }

    private void OnDisable()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.PauseStatusChanged -= OnPauseStatusChanged;
        }
    }

    private void FixedUpdate()
    {
        UpdateEnemyMovement();

        UpdateEnemyFiring();
    }

    private void OnPauseStatusChanged()
    {
        if (!SceneManager.Instance.GamePaused)
        {
            m_EngineAudioSource.Play();
        }
        else if (SceneManager.Instance.GamePaused)
        {
            m_EngineAudioSource.Pause();
        }
    }

    private void UpdateEnemyMovement()
    {
        // Set rotating time randomly
        if (Utilities.CalculateOdds(m_RotationDice))
        {
            RotateEnemy(Random.Range(0, 360));
        }

        // Make small enemy ships find player regularly
        if (m_EnemyType == ObjectType.SMALL_ENEMY && Utilities.CalculateOdds(m_TurnToPlayerDice))
        {
            if (GameController.Instance.Player != null)
            {
                // Rotate enemy ship towards player
                Vector3 enemyToPlayerVector = GameController.Instance.Player.transform.position - transform.position;
                enemyToPlayerVector.Normalize();
                float rotationZ = Mathf.Atan2(enemyToPlayerVector.y, enemyToPlayerVector.x) * Mathf.Rad2Deg;
                RotateEnemy(rotationZ - 90);
            }
        }
    }

    private void RotateEnemy(float rotationZ)
    {
        // Turn ship
        gameObject.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        // Move ship forward
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * m_EnemySpeed;
    }

    private void UpdateEnemyFiring()
    {
        // Set enemy firing time randomly
        if (Utilities.CalculateOdds(m_EnemyFireDice))
        {
            // Play firing sound on muzzle
            AudioSource muzzleAudioSource = m_Muzzle1.GetComponent<AudioSource>();
            if (muzzleAudioSource != null)
            {
                muzzleAudioSource.Play();
            }

            // Fire bullets
            Vector3 shipVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            m_FireController.FireBullet(BulletPoolManager.Instance.EnemyBulletPool, m_Muzzle1.transform, shipVelocity);
            m_FireController.FireBullet(BulletPoolManager.Instance.EnemyBulletPool, m_Muzzle2.transform, shipVelocity);
        }
    }
}