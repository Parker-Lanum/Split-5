using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [Header("Clips")]
    public AudioClip nuclearWarning;
    public AudioClip piano;

    [Header("Sources")]
    public AudioSource warningSource;
    public AudioSource pianoSource;

    [Header("Level Volumes")]
    public float[] warningVolumes = { 1.0f, 0.75f, 0.5f, 0.25f, 0.05f };
    public float[] pianoVolumes = { 0.1f, 0.25f, 0.45f, 0.7f, 1.0f };

    void Awake()
    {
        Instance = this;

        if (warningSource == null)
            warningSource = gameObject.AddComponent<AudioSource>();

        if (pianoSource == null)
            pianoSource = gameObject.AddComponent<AudioSource>();

        warningSource.playOnAwake = false;
        pianoSource.playOnAwake = false;

        warningSource.loop = true;
        pianoSource.loop = true;
    }

    void Start()
    {
        if (nuclearWarning != null)
        {
            warningSource.clip = nuclearWarning;
            warningSource.Play();
        }

        if (piano != null)
        {
            pianoSource.clip = piano;
            pianoSource.Play();
        }

        SetLevelAudio(0);
    }

    public void SetLevelAudio(int levelIndex)
    {
        levelIndex = Mathf.Clamp(levelIndex, 0, warningVolumes.Length - 1);

        warningSource.volume = warningVolumes[levelIndex];
        pianoSource.volume = pianoVolumes[levelIndex];
    }
}

