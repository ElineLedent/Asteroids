public class PlayerDestroyer : ObjectDestroyer
{
    protected override void OnObjectDestroyed()
    {
        GameController.Instance.OnPlayerDestroyed();
    }
}