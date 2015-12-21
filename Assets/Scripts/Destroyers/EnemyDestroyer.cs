public class EnemyDestroyer : ObjectDestroyer
{
    protected override void OnObjectDestroyed()
    {
        GameController.Instance.OnEnemyDestroyed(this.gameObject.transform.position, m_ObjectType);
    }
}