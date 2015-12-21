using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_EnemyShipLargePrefab;

    [SerializeField]
    private GameObject m_EnemyShipSmallPrefab;

    [SerializeField]
    private int m_SmallEnemyOdds = 20;

    [SerializeField]
    private int m_RespawnOdds = 2;

    [SerializeField]
    private int m_MaxEnemyRespawnDelay = 3;

    [SerializeField]
    private int m_EnemySpawningChanceMax = 6000;

    [SerializeField]
    private int m_EnemySpawningChanceMin = 3000;

    [SerializeField]
    private int m_EnemyMax = 2;

    private int m_EnemySpawningChance = 0;
    private int m_EnemyCounter = 0;

    private void OnEnable()
    {
        // Randomely determine the likeliness of enemies for current level
        m_EnemySpawningChance = Random.Range(m_EnemySpawningChanceMin, m_EnemySpawningChanceMax);

        enabled &= DebugUtilities.Verify(m_EnemyShipLargePrefab != null, "Large enemy ship prefab not assigned");
        enabled &= DebugUtilities.Verify(m_EnemyShipSmallPrefab != null, "Small enemy ship prefab not assigned");
    }

    private void Update()
    {
        if (!SceneManager.Instance.GamePaused)
        {
            // Calculate random moments for spawning enemies (1/odds possibility)
            if (m_EnemyCounter < m_EnemyMax && Utilities.CalculateOdds(m_EnemySpawningChance))
            {
                SpawnEnemy();
                ++m_EnemyCounter;
            }
        }
    }

    private void SpawnEnemy()
    {
        // Create a small possibility that enemycraft is small, else spawn large enemycraft
        if (Utilities.CalculateOdds(m_SmallEnemyOdds))
        {
            Instantiate(m_EnemyShipSmallPrefab, Utilities.CalculateRandomSpawningPoint(), Quaternion.identity);
        }
        else
        {
            Instantiate(m_EnemyShipLargePrefab, Utilities.CalculateRandomSpawningPoint(), Quaternion.identity);
        }
    }

    public void OnEnemyDestroyed()
    {
        --m_EnemyCounter;

        // Create a one in respawn odds possibility that enemy will not respawn
        if (m_EnemyCounter < m_EnemyMax && Utilities.CalculateOdds(m_RespawnOdds))
        {
            if (enabled)
            {
                // Add random delay
                Invoke("SpawnEnemy", Random.Range(0, m_MaxEnemyRespawnDelay));
                ++m_EnemyCounter;
            }
        }
    }
}