using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isPlaying = false;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Toggle play/pause when the 'M' key is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isPlaying)
            {
                audioSource.Pause();  // Pause the sound if it's playing
                isPlaying = false;
            }
            else
            {
                audioSource.Play();   // Play the sound if it's paused
                isPlaying = true;
            }
        }
    }
}
