using UnityEngine;

public class StartSceneSoundManager : MonoBehaviour
{
    public static StartSceneSoundManager Instance { get; private set; }

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip creditOpenClip;
    public AudioClip startAlarmClip;

    [Header("Volumes")]
    [Range(0f, 1f)] public float creditOpenVolume = 0.8f;
    [Range(0f, 1f)] public float startAlarmVolume = 1.0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f;
    }

    public void PlayCreditOpen()
    {
        PlaySFX(creditOpenClip, creditOpenVolume);
    }

    public void PlayStartAlarm()
    {
        PlaySFX(startAlarmClip, startAlarmVolume);
    }

    void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }
}
