using UnityEngine;

public enum ObjectType { PLAYER, LARGE_ASTEROID, MEDIUM_ASTEROID, SMALL_ASTEROID, LARGE_ENEMY, SMALL_ENEMY };

/// <summary>
/// Singleton controller class for main gameplay logic
/// </summary>
[RequireComponent(typeof(PlayerSpawner))]
[RequireComponent(typeof(AsteroidSpawner))]
[RequireComponent(typeof(EnemySpawner))]
public class GameController : MonoSingleton<GameController>
{
    [SerializeField]
    private int m_NumberOfPiecesBreakingOff = 2;

    [SerializeField]
    private float m_SpawnNewPlayerDelay = 1;

    [SerializeField]
    private float m_PlayerFailedRestartDelay = 2;

    [SerializeField]
    private float m_ProceedToNextLevelDelay = 1;

    private GameObject m_Player;

    private PlayerSpawner m_PlayerSpawner;
    private AsteroidSpawner m_AsteroidSpawner;
    private EnemySpawner m_EnemySpawner;

    private bool m_LevelSucceeded = false;

    public GameObject Player
    {
        get
        {
            return m_Player;
        }
    }

    protected GameController()
    {
    }

    private void Start()
    {
        m_PlayerSpawner = gameObject.GetComponent<PlayerSpawner>();
        m_AsteroidSpawner = gameObject.GetComponent<AsteroidSpawner>();
        m_EnemySpawner = gameObject.GetComponent<EnemySpawner>();

        SpawnNewPlayer();
        m_AsteroidSpawner.SpawnRandomAsteroids(GameStatistics.Instance.StartingNumberOfAsteroids);

        // Check if player was destroyed after last asteroid was destroyed
        if (GameStatistics.Instance.PlayerLives <= 0)
        {
            // Load main menu
            ReloadLevel();
        }
    }

    private void SpawnNewPlayer()
    {
        m_Player = m_PlayerSpawner.SpawnPlayer();
    }

    public void OnPlayerDestroyed()
    {
        // Lower life count
        --GameStatistics.Instance.PlayerLives;

        m_Player = null;

        // Check player lives
        if (GameStatistics.Instance.PlayerLives > 0)
        {
            // Respawn player ship
            Invoke("SpawnNewPlayer", m_SpawnNewPlayerDelay);
        }
        else
        {
            // Display retry screen
            Invoke("ReloadLevel", m_PlayerFailedRestartDelay);
        }
    }

    public void OnEnemyDestroyed(Vector3 enemyPosition, ObjectType enemyType)
    {
        AddAndDisplayPoints(enemyPosition, enemyType);

        m_EnemySpawner.OnEnemyDestroyed();
    }

    public void OnAsteroidDestroyed(Vector3 asteroidPosition, ObjectType asteroidType)
    {
        // Count score, and spawn smaller asteroids if necessary
        AddAndDisplayPoints(asteroidPosition, asteroidType);

        m_AsteroidSpawner.OnAsteroidDestroyed();

        if (asteroidType == ObjectType.LARGE_ASTEROID)
        {
            for (int i = 0; i < m_NumberOfPiecesBreakingOff; ++i)
            {
                m_AsteroidSpawner.SpawnAsteroid(asteroidPosition, ObjectType.MEDIUM_ASTEROID);
            }
        }
        else if (asteroidType == ObjectType.MEDIUM_ASTEROID)
        {
            for (int i = 0; i < m_NumberOfPiecesBreakingOff; ++i)
            {
                m_AsteroidSpawner.SpawnAsteroid(asteroidPosition, ObjectType.SMALL_ASTEROID);
            }
        }

        if (m_AsteroidSpawner.AsteroidCounter == 0)
        {
            m_LevelSucceeded = true;
            Invoke("ReloadLevel", m_ProceedToNextLevelDelay);
        }
    }

    private void AddAndDisplayPoints(Vector3 position, ObjectType type)
    {
        int lastEarnedPoints = GameStatistics.Instance.AddScore(type);

        UIGame.Instance.DisplayLastEarnedPoints(lastEarnedPoints, position);
    }

    private void ReloadLevel()
    {
        // Set to next level, keep current score
        if (m_LevelSucceeded && GameStatistics.Instance.PlayerLives > 0)
        {
            ++GameStatistics.Instance.LevelNumber;
            SceneManager.Instance.LoadGameLevel();
        }
        // Display end of game menu
        else
        {
            // Check for top score
            int scoreRank = TopScoreManager.Instance.CheckForTopScore(GameStatistics.Instance.Score);

            if (scoreRank != -1)
            {
                // Display top score
                UIGame.Instance.DisplayTopScore(++scoreRank);
            }
            else
            {
                // Display retry screen
                UIGame.Instance.DisplayRetryScreen();
            }
        }
    }
}