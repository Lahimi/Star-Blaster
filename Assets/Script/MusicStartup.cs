using UnityEngine;

public class MusicStartup : MonoBehaviour
{
    [SerializeField] AudioClip levelBGM;

    void Start()
    {
        // Find the persistent player and force it to play THIS scene's music
        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.PlaySpecificMusic(levelBGM);
        }
    }
}