using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Muzzle;

    [SerializeField]
    private Material[] m_PlayerMaterials;

    [SerializeField]
    private Material[] m_PlayerTransparantMaterials;

    [SerializeField]
    private float m_PlayerSpeed = 20f;

    [SerializeField]
    private float m_PlayerRotationSpeed = 400f;

    [SerializeField]
    private float m_TotalFlickerTime = 2f;

    [SerializeField]
    private float m_FlickerOnOffTime = 0.07f;

    private float m_InitialEngineVolume = 0f;
    private float m_FlickerOnOffTimeCounter = 0f;
    private bool m_PlayerIsFlickering = false;
    private bool m_FlickerSwitch = false;

    private MeshRenderer m_PlayerRenderer;

    private AudioSource m_EngineAudioSource;

    private FireController m_FireController = new FireController();

    private void OnEnable()
    {
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        m_EngineAudioSource = gameObject.GetComponent<AudioSource>();

        enabled &= DebugUtilities.Verify(m_PlayerMaterials != null, "Player materials not assigned");
        enabled &= DebugUtilities.Verify(m_PlayerTransparantMaterials != null, "Player transparant materials not assigned");
        enabled &= DebugUtilities.Verify(m_Muzzle != null, "Player muzzle child object not assigned");

        m_PlayerRenderer = gameObject.transform.GetComponentInChildren<MeshRenderer>();
        enabled &= DebugUtilities.Verify(m_PlayerRenderer != null, "Player renderer not found");

        SceneManager.Instance.PauseStatusChanged += OnPauseStatusChanged;
    }

    private void OnDisable()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.PauseStatusChanged -= OnPauseStatusChanged;
        }
    }

    private void Start()
    {
        // Check if player is holding down the forward button
        OnPauseStatusChanged();

        // Make player indestructible
        gameObject.layer = LayerMask.NameToLayer("Indestructible");
        m_PlayerIsFlickering = true;
        Invoke("MakePlayerDestructible", m_TotalFlickerTime);

        m_InitialEngineVolume = m_EngineAudioSource.volume;
        m_EngineAudioSource.volume = 0;
    }

    private void Update()
    {
        if (!SceneManager.Instance.GamePaused)
        {
            UpdatePlayerFiring();

            if (m_PlayerIsFlickering)
            {
                FlickerPlayer();
            }
        }
    }

    private void FixedUpdate()
    {
        UpdatePlayerMovement();
    }

    private void OnPauseStatusChanged()
    {
        if (!SceneManager.Instance.GamePaused && Input.GetButton("Vertical"))
        {
            m_EngineAudioSource.Play();
        }
        else
        {
            m_EngineAudioSource.Pause();
        }
    }

    private void FlickerPlayer()
    {
        m_FlickerOnOffTimeCounter += Time.deltaTime;

        if (m_FlickerOnOffTimeCounter >= m_FlickerOnOffTime)
        {
            m_FlickerSwitch = !m_FlickerSwitch;

            m_PlayerRenderer.materials = m_FlickerSwitch ? m_PlayerTransparantMaterials : m_PlayerMaterials;

            m_FlickerOnOffTimeCounter = 0f;
        }
    }

    private void MakePlayerDestructible()
    {
        // Make player destructible
        gameObject.layer = LayerMask.NameToLayer("PlayerShip");
        m_PlayerIsFlickering = false;
        m_PlayerRenderer.materials = m_PlayerMaterials;
    }

    private void UpdatePlayerMovement()
    {
        float inputVertical = Input.GetAxis("Vertical");
        // Forward movement is achieved through physics for realistic movement
        gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, inputVertical * m_PlayerSpeed));

        // Rotation is not achieved through physics but through setting the transform due to precision requirements
        float rotation = Input.GetAxis("Horizontal") * m_PlayerRotationSpeed * Time.deltaTime * (-1);
        transform.Rotate(0, 0, rotation);

        // Play engine sound
        UpdateEngineSound(inputVertical);
    }

    private void UpdateEngineSound(float inputVertical)
    {
        float engineVolume = m_EngineAudioSource.volume;

        // Increase volume
        if (inputVertical > 0)
        {
            if (!m_EngineAudioSource.isPlaying)
            {
                m_EngineAudioSource.Play();
            }

            engineVolume += Time.fixedDeltaTime;
        }
        // Reduce volume
        else if (m_EngineAudioSource.isPlaying)
        {
            engineVolume -= Time.fixedDeltaTime;

            if (engineVolume <= 0)
            {
                m_EngineAudioSource.Pause();
            }
        }

        m_EngineAudioSource.volume = Mathf.Clamp(engineVolume, 0, m_InitialEngineVolume);
    }

    private void UpdatePlayerFiring()
    {
        // Set player firing through player input
        if (Input.GetButtonDown("Fire1"))
        {
            AudioSource muzzleAudioSource = m_Muzzle.GetComponent<AudioSource>();
            // Play bullet firing sound on muzzle
            if (DebugUtilities.Verify(muzzleAudioSource != null, "Audiosource on player ship muzzle not found"))
            {
                muzzleAudioSource.Play();
            }

            // Fire Bullet
            Vector3 shipVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            m_FireController.FireBullet(BulletPoolManager.Instance.PlayerBulletPool, m_Muzzle.transform, shipVelocity);
        }
    }
}