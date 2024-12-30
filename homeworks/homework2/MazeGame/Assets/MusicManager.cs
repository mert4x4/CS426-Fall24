using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource; // Assign an AudioSource with your music clip in the Inspector
    private bool isMusicPlaying = true; // Tracks whether the music is playing

    private static MusicManager instance; // Singleton instance for persistence

    void Awake()
    {
        // Singleton pattern to ensure only one MusicManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
    }

    void Start()
    {
        // Ensure the AudioSource is assigned and music starts playing
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
            isMusicPlaying = true;
        }
        else if (musicSource == null)
        {
            Debug.LogError("MusicManager: No AudioSource found! Please assign one in the Inspector.");
        }
    }

    void Update()
    {
        // Check if the "M" key is pressed to toggle music
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusic();
        }
    }

    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            if (isMusicPlaying)
            {
                musicSource.Pause(); // Pause music
                Debug.Log("Music Paused.");
            }
            else
            {
                musicSource.Play(); // Resume music
                Debug.Log("Music Resumed.");
            }

            isMusicPlaying = !isMusicPlaying; // Toggle the state
        }
        else
        {
            Debug.LogError("MusicManager: No AudioSource assigned.");
        }
    }
}
