public class AsteroidDestroyer : ObjectDestroyer
{
    protected override void OnObjectDestroyed()
    {
        GameController.Instance.OnAsteroidDestroyed(this.gameObject.transform.position, m_ObjectType);
    }
}