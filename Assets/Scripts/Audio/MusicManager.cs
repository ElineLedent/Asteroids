using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<AudioSource>().Play();

        SceneManager.Instance.PauseStatusChanged += OnPauseStatusChanged;
    }

    private void OnDisable()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.PauseStatusChanged -= OnPauseStatusChanged;
        }
    }

    private void OnPauseStatusChanged()
    {
        if (SceneManager.Instance.GamePaused)
        {
            // Turn background music on
            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            // Turn background music off
            gameObject.GetComponent<AudioSource>().Pause();
        }
    }
}