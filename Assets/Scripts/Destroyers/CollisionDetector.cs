using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public delegate void CollisionDetectedEventHandler();

    public event CollisionDetectedEventHandler CollisionDetected;

    private void OnCollisionEnter2D(Collision2D collider)
    {
        OnCollisionDetected();
    }

    private void OnCollisionDetected()
    {
        if (CollisionDetected != null)
        {
            CollisionDetected();
        }
    }
}