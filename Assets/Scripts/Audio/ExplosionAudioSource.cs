using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExplosionAudioSource : MonoBehaviour
{
    private AudioSource m_AudioSource;

    private void OnEnable()
    {
        m_AudioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!m_AudioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}