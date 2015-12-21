using UnityEngine;

public class Utilities
{
    // Calculate random spawning point at edge of screen
    // Possible spawning points are situated between sides of screen and one eight of width/length
    public static Vector3 CalculateRandomSpawningPoint()
    {
        float positionX = 0f;
        float positionY = 0f;

        // In viewport coordinates, one of the two coordinates (x or y) needs to be between 0 and 1.
        // The other coordinate needs to be between 0 and 0.125 or between 0.875 and 1
        if (Random.Range(0, 2) == 0)
        {
            positionX = Random.Range(0f, 1f);
            positionY = CalculateOtherCoordinate();
        }
        else
        {
            positionY = Random.Range(0f, 1f);
            positionX = CalculateOtherCoordinate();
        }

        // Switch viewport values to worldpoints
        Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(positionX, positionY, 0));
        // Force world z position to zero
        position.z = 0;

        return position;
    }

    public static Vector2 RandomizeForce(float minSpeed, float maxSpeed)
    {
        // First search random force magnitude between passed in min and max values
        float totalForce = Random.Range(minSpeed, maxSpeed);

        // Then randomize the forces along the X and Y axes through the formula A^2 = B^2 + C^2
        float x = Random.Range(-totalForce, totalForce);
        float y = Mathf.Sqrt(totalForce * totalForce - x * x);

        return new Vector2(x, y);
    }

    public static bool CalculateOdds(int diceThrow)
    {
        // Increment diceThrow value by 1 since Random.Range(int min [inclusive], int max [exclusive]
        ++diceThrow;

        return (Random.Range(1, diceThrow) == diceThrow - 1);
    }

    private static float CalculateOtherCoordinate()
    {
        const float RANDOM_SPAWN_RANGE = 0.125f;

        float otherCoordinate = 0f;

        if (Random.Range(0, 2) == 0)
        {
            otherCoordinate = Random.Range(0f, RANDOM_SPAWN_RANGE);
        }
        else
        {
            otherCoordinate = Random.Range(1f - RANDOM_SPAWN_RANGE, 1f);
        }

        return otherCoordinate;
    }
}