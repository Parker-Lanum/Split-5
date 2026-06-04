using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Music")]
    public AudioClip nuclearWarning;
    public AudioClip piano;

    [Header("Player")]
    public AudioClip walking;
    //public AudioClip jumping;

    [Header("Volumes")]
    [Range(0f, 1f)] public float warningVolume = 1.0f;
    [Range(0f, 1f)] public float pianoVolume = 0.1f;
    [Range(0f, 1f)] public float playerVolume = 1.0f

    void Start()
    {
        if (nuclearWarning != null) SoundManager.Instance.PlayMusic(nuclearWarning, warningVolume);
        if (piano != null) SoundManager.Instance.PlayMusic(piano, pianoVolume);
    }

    
}

