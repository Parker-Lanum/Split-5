using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [Header("Clips")]
    public AudioClip nuclearWarning;
    public AudioClip piano;

    [Header("Level Volumes")]
    public float[] warningVolumes = { 1.0f, 0.75f, 0.5f, 0.25f, 0.0f };
    public float[] pianoVolumes = { 0.1f, 0.25f, 0.45f, 0.7f, 1.0f };

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

