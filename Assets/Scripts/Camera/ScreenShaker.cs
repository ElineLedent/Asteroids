using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField]
    private float m_PlayerDestrCamShake = 0.7f;

    [SerializeField]
    private float m_LargeEnemyDestrCamShake = 0.3f;

    [SerializeField]
    private float m_SmallEnemyDestrCamShake = 0.25f;

    [SerializeField]
    private float m_SmallAsteroidDestrCamShake = 0.25f;

    [SerializeField]
    private float m_MediumAsteriodDestrCamShake = 0.3f;

    [SerializeField]
    private float m_LargeAsteroidDestrCamShake = 0.4f;

    private Vector3 m_OriginalCameraPosition = Vector3.zero;
    private float m_FadingSpeed = 0.005f;
    private float m_ShakeIntensity = 0;

    private void Start()
    {
        m_OriginalCameraPosition = gameObject.transform.position;
    }

    private void Update()
    {
        if (!SceneManager.Instance.GamePaused)
        {
            this.gameObject.transform.position = m_OriginalCameraPosition;

            if (m_ShakeIntensity > 0)
            {
                Vector3 targetPosition = m_OriginalCameraPosition;
                targetPosition.x = CalculateShake();
                targetPosition.y = CalculateShake();
                this.gameObject.transform.position = targetPosition;
                m_ShakeIntensity -= m_FadingSpeed;
            }
        }
    }

    public void ShakeCamera(ObjectType type)
    {
        float shakeIntensity = GetShakeIntensity(type);
        // If a newly received shake intensity value is larger than the current one, assign the new value
        if (shakeIntensity > m_ShakeIntensity)
        {
            m_ShakeIntensity = shakeIntensity;
        }
    }

    private float GetShakeIntensity(ObjectType type)
    {
        float shakeIntensity = 0f;

        switch (type)
        {
            case ObjectType.LARGE_ASTEROID:
                shakeIntensity = m_LargeAsteroidDestrCamShake;
                break;

            case ObjectType.MEDIUM_ASTEROID:
                shakeIntensity = m_MediumAsteriodDestrCamShake;
                break;

            case ObjectType.SMALL_ASTEROID:
                shakeIntensity = m_SmallAsteroidDestrCamShake;
                break;

            case ObjectType.PLAYER:
                shakeIntensity = m_PlayerDestrCamShake;
                break;

            case ObjectType.LARGE_ENEMY:
                shakeIntensity = m_LargeEnemyDestrCamShake;
                break;

            case ObjectType.SMALL_ENEMY:
                shakeIntensity = m_SmallEnemyDestrCamShake;
                break;

            default:
                Debug.Log("Invalid object type");
                break;
        }

        return shakeIntensity;
    }

    private float CalculateShake()
    {
        return Random.value * m_ShakeIntensity * 2 - m_ShakeIntensity;
    }
}