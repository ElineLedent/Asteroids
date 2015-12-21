using UnityEngine;

public class ExplosionSoundManager : MonoSingleton<ExplosionSoundManager>
{
    private const int POOL_START_AMOUNT = 20;

    [SerializeField]
    private GameObject m_ExplosionAudioSourcePrefab;

    [SerializeField]
    private AudioClip m_PlayerShipExplosion;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_PlayerExplosionVol = 0.8f;

    [SerializeField]
    private AudioClip m_SmallAsteroidExplosion;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_SmallAstExplosionVol = 0.5f;

    [SerializeField]
    private AudioClip m_MediumAsteroidExplosion;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_MediumAstExplosionVol = 0.7f;

    [SerializeField]
    private AudioClip m_LargeAsteroidExplosion;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_LargeAstExplosionVol = 1f;

    [SerializeField]
    private AudioClip m_EnemyShipExplosion;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_SmallEnemyExplosionVol = 0.3f;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_LargeEnemyExplosionVol = 0.4f;

    private ObjectPool m_AudioSourcePool;

    protected ExplosionSoundManager()
    {
    }

    private void OnEnable()
    {
        if (DebugUtilities.Verify(m_ExplosionAudioSourcePrefab != null, "Audio source prefab not assigned"))
        {
            m_AudioSourcePool = new ObjectPool(m_ExplosionAudioSourcePrefab, POOL_START_AMOUNT);
        }
    }

    public void PlayExplosionSound(ObjectType type, Vector3 location)
    {
        switch (type)
        {
            case ObjectType.SMALL_ASTEROID:
                PlayAudioClip(m_SmallAsteroidExplosion, location, m_SmallAstExplosionVol);
                break;

            case ObjectType.MEDIUM_ASTEROID:
                PlayAudioClip(m_MediumAsteroidExplosion, location, m_MediumAstExplosionVol);
                break;

            case ObjectType.LARGE_ASTEROID:
                PlayAudioClip(m_LargeAsteroidExplosion, location, m_LargeAstExplosionVol);
                break;

            case ObjectType.PLAYER:
                PlayAudioClip(m_PlayerShipExplosion, location, m_PlayerExplosionVol);
                break;

            case ObjectType.SMALL_ENEMY:
                PlayAudioClip(m_EnemyShipExplosion, location, m_SmallEnemyExplosionVol);
                break;

            case ObjectType.LARGE_ENEMY:
                PlayAudioClip(m_EnemyShipExplosion, location, m_LargeEnemyExplosionVol);
                break;

            default:
                Debug.Log("Invalid object type");
                break;
        }
    }

    private void PlayAudioClip(AudioClip clip, Vector3 location, float volume = 1.0f, float maxDistance = 500f)
    {
        GameObject audioSourceObject = m_AudioSourcePool.GetFreePoolObject();

        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();

        if (audioSource != null && DebugUtilities.Verify(clip != null, "Audioclip not assigned"))
        {
            audioSource.playOnAwake = false;
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.maxDistance = maxDistance;

            audioSourceObject.SetActive(true);

            audioSource.Play();
        }
    }
}