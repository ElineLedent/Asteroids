using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDestroyer : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, gameObject.GetComponent<ParticleSystem>().duration);
    }
}