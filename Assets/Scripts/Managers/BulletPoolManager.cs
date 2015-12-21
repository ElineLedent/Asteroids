using UnityEngine;

public class BulletPoolManager : MonoSingleton<BulletPoolManager>
{
    private const int POOL_START_AMOUNT = 16;

    [SerializeField]
    private GameObject m_PlayerBulletPrefab;

    [SerializeField]
    private GameObject m_EnemyBulletPrefab;

    private ObjectPool m_PlayerBulletPool;
    private ObjectPool m_EnemyBulletPool;

    protected BulletPoolManager()
    {
    }

    public ObjectPool PlayerBulletPool
    {
        get
        {
            Debug.Assert(m_PlayerBulletPool != null, "Player bullet pool not yet created");
            return m_PlayerBulletPool;
        }
    }

    public ObjectPool EnemyBulletPool
    {
        get
        {
            Debug.Assert(m_EnemyBulletPool != null, "Enemy bullet pool not yet created");
            return m_EnemyBulletPool;
        }
    }

    private void OnEnable()
    {
        enabled &= DebugUtilities.Verify(m_PlayerBulletPrefab != null, "Player bullet prefab not assigned");
        enabled &= DebugUtilities.Verify(m_EnemyBulletPrefab != null, "Enemy bullet prefab not assigned");

        if (enabled)
        {
            m_PlayerBulletPool = new ObjectPool(m_PlayerBulletPrefab, POOL_START_AMOUNT);
            m_EnemyBulletPool = new ObjectPool(m_EnemyBulletPrefab, POOL_START_AMOUNT);
        }
    }
}