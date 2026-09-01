using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip bossMusic;

    public static MusicPlayer Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySpecificMusic(AudioClip newClip, bool loop = true)
    {
        if (audioSource == null || newClip == null) return;
        
        // If the song is already playing, don't restart it!
        if (audioSource.clip == newClip && audioSource.isPlaying) return;

        audioSource.clip = newClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayBossMusic()
    {
        if (bossMusic != null) PlaySpecificMusic(bossMusic);
    }

    public void StopMusic()
    {
        if (audioSource != null) audioSource.Stop();
    }
}