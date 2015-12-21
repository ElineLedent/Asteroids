using UnityEngine;

public class GameStatistics : MonoSingleton<GameStatistics>
{
    [SerializeField]
    private int m_StartingLives = 3;

    [SerializeField]
    private int m_LevelsBeforeGainingLife = 4;

    [SerializeField]
    private int m_ExtraAsteroids = 3;

    [SerializeField]
    private int m_PointsLargeAsteroid = 20;

    [SerializeField]
    private int m_PointsMediumAsteroid = 50;

    [SerializeField]
    private int m_PointsSmallAsteroid = 100;

    [SerializeField]
    private int m_PointsLargeEnemy = 50;

    [SerializeField]
    private int m_PointsSmallEnemy = 150;

    public delegate void ValueChangedEventHandler(ValueType type, int value);

    public event ValueChangedEventHandler ValueChanged;

    private int m_LevelNumber = 1;
    private int m_Score = 0;
    private int m_PlayerLives;

    public int LevelNumber
    {
        set
        {
            m_LevelNumber = value;
            // Add a life every 4 levels
            if (m_LevelNumber % m_LevelsBeforeGainingLife == 0)
            {
                ++PlayerLives;
            }
        }
        get { return m_LevelNumber; }
    }

    public int Score
    {
        set
        {
            m_Score = value;
            OnValueChanged(ValueType.SCORE, m_Score);
        }
        get { return m_Score; }
    }

    public int PlayerLives
    {
        set
        {
            m_PlayerLives = value;
            OnValueChanged(ValueType.LIVES, m_PlayerLives);
        }
        get { return m_PlayerLives; }
    }

    public int StartingNumberOfAsteroids
    {
        get { return LevelNumber + m_ExtraAsteroids; }
    }

    protected GameStatistics()
    {
    }

    private void Start()
    {
        m_PlayerLives = m_StartingLives;
    }

    private void OnValueChanged(ValueType type, int value)
    {
        if (ValueChanged != null)
        {
            ValueChanged(type, value);
        }
    }

    public int AddScore(ObjectType type)
    {
        int points = CalculatePoints(type);
        Score += points;

        return points;
    }

    private int CalculatePoints(ObjectType type)
    {
        int points = 0;

        switch (type)
        {
            case ObjectType.SMALL_ASTEROID:
                points = m_PointsSmallAsteroid;
                break;

            case ObjectType.MEDIUM_ASTEROID:
                points = m_PointsMediumAsteroid;
                break;

            case ObjectType.LARGE_ASTEROID:
                points = m_PointsLargeAsteroid;
                break;

            case ObjectType.LARGE_ENEMY:
                points = m_PointsLargeEnemy;
                break;

            case ObjectType.SMALL_ENEMY:
                points = m_PointsSmallEnemy;
                break;

            default:
                Debug.Log("Invalid object type");
                break;
        }

        return points;
    }

    public void ResetGameStatistics()
    {
        m_LevelNumber = 1;
        m_PlayerLives = m_StartingLives;
        m_Score = 0;
    }
}