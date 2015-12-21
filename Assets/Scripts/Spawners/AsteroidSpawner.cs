using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns asteroids at either random or specific positions
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_AsteroidsLargePrefab;

    [SerializeField]
    private List<GameObject> m_AsteroidsMediumPrefab;

    [SerializeField]
    private List<GameObject> m_AsteroidsSmallPrefab;

    [SerializeField]
    private Vector2 m_AsteroidLargeForceRange = new Vector2(150f, 150f);

    [SerializeField]
    private Vector2 m_AsteroidMediumForceRange = new Vector2(100f, 200f);

    [SerializeField]
    private Vector2 m_AsteroidSmallForceRange = new Vector2(100f, 300f);

    private int m_AsteroidCounter = 0;

    public int AsteroidCounter
    {
        get { return m_AsteroidCounter; }
    }

    public void SpawnRandomAsteroids(int numberOfAsteroids)
    {
        for (int i = 0; i < numberOfAsteroids; ++i)
        {
            SpawnAsteroid(Utilities.CalculateRandomSpawningPoint(), ObjectType.LARGE_ASTEROID);
        }
    }

    public void SpawnAsteroid(Vector3 asteroidPosition, ObjectType asteroidType)
    {
        GameObject currentAsteroid = InstantiateAsteroidPrefab(asteroidPosition, asteroidType);

        if (currentAsteroid != null)
        {
            float randomRotation = Random.Range(0, 360);
            currentAsteroid.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

            Rigidbody2D asteroidRB = currentAsteroid.GetComponent<Rigidbody2D>();

            if (DebugUtilities.Verify(asteroidRB != null, "Failed to find asteroid rigidbody"))
            {
                Vector2 force = CalculateForce(asteroidType);
                asteroidRB.AddForce(force);
            }

            // Count asteroids that are spawned
            ++m_AsteroidCounter;
        }
    }

    private GameObject InstantiateAsteroidPrefab(Vector3 asteroidPosition, ObjectType asteroidType)
    {
        GameObject asteroidObject = null;
        List<GameObject> asteroidList = null;

        switch (asteroidType)
        {
            case ObjectType.LARGE_ASTEROID:
                asteroidList = m_AsteroidsLargePrefab;
                break;

            case ObjectType.MEDIUM_ASTEROID:
                asteroidList = m_AsteroidsMediumPrefab;
                break;

            case ObjectType.SMALL_ASTEROID:
                asteroidList = m_AsteroidsSmallPrefab;
                break;

            default:
                Debug.Log("Invalid object type");
                break;
        }

        if (asteroidList != null)
        {
            int shape = Random.Range(0, asteroidList.Count);
            asteroidObject = (GameObject)Instantiate(asteroidList[shape], asteroidPosition, Quaternion.identity);
        }

        Debug.Assert(asteroidObject != null, "Failed to spawn asteroid");

        return asteroidObject;
    }

    private Vector2 CalculateForce(ObjectType asteroidType)
    {
        Vector2 force = new Vector2(0f, 0f);

        switch (asteroidType)
        {
            case ObjectType.LARGE_ASTEROID:
                force = Utilities.RandomizeForce(m_AsteroidLargeForceRange.x, m_AsteroidLargeForceRange.y);
                break;

            case ObjectType.MEDIUM_ASTEROID:
                force = Utilities.RandomizeForce(m_AsteroidMediumForceRange.x, m_AsteroidMediumForceRange.y);
                break;

            case ObjectType.SMALL_ASTEROID:
                force = Utilities.RandomizeForce(m_AsteroidSmallForceRange.x, m_AsteroidSmallForceRange.y);
                break;

            default:
                Debug.Log("Invalid object type");
                break;
        }

        return force;
    }

    public void OnAsteroidDestroyed()
    {
        --m_AsteroidCounter;
    }
}