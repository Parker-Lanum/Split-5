using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [Header("Clips")]
    public AudioClip nuclearWarning;
    public AudioClip piano;

    [Header("Level Volumes")]
    public float[] warningVolumes =
{
    0.90f, // Level 1
    0.70f,
    0.55f,
    0.45f,
    0.30f,
    0.20f,
    0.10f,
    0.05f,
    0.03f,
    0.01f,
    0.00f  // Level 11
};

    public float[] pianoVolumes =
    {
    0.10f, // Level 1
    0.25f,
    0.40f,
    0.50f,
    0.60f,
    0.70f,
    0.80f,
    0.85f,
    0.90f,
    0.95f,
    1.00f  // Level 11
};

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("AudioController: No SoundManager found in scene.");
            return;
        }

        SoundManager.Instance.PlayWarning(nuclearWarning, warningVolumes[0]);
        SoundManager.Instance.PlayPiano(piano, pianoVolumes[0]);

        SetLevelAudio(0);
    }

    public void SetLevelAudio(int levelIndex)
    {
        if (SoundManager.Instance == null) return;

        int maxIndex = Mathf.Min(warningVolumes.Length, pianoVolumes.Length) - 1;
        if (maxIndex < 0) return;

        levelIndex = Mathf.Clamp(levelIndex, 0, maxIndex);

        SoundManager.Instance.SetWarningVolume(warningVolumes[levelIndex]);
        SoundManager.Instance.SetPianoVolume(pianoVolumes[levelIndex]);
    }
}

