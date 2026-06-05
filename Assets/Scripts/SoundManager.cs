using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources (auto-created if left empty)")]
    public AudioSource warningSource;
    public AudioSource pianoSource;
    public AudioSource sfxSource;
    public AudioSource loopSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        pianoSource = EnsureSource(pianoSource);
        warningSource = EnsureSource(warningSource);
        sfxSource = EnsureSource(sfxSource);
        loopSource = EnsureSource(loopSource);
    }

    AudioSource EnsureSource(AudioSource src)
    {
        if (src == null) src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake  = false;
        src.spatialBlend = 0f;   // 2D - not attenuated by distance from the listener
        return src;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }


    public void PlayWarning(AudioClip clip, float volume = 1f)
    {
        if (clip == null || warningSource == null) return;

        warningSource.clip = clip;
        warningSource.loop = true;
        warningSource.volume = Mathf.Clamp01(volume);
        warningSource.Play();
    }

    public void PlayPiano(AudioClip clip, float volume = 1f)
    {
        if (clip == null || pianoSource == null) return;

        pianoSource.clip = clip;
        pianoSource.loop = true;
        pianoSource.volume = Mathf.Clamp01(volume);
        pianoSource.Play();
    }

    public void StartLoop(AudioClip clip, float volume = 1f)
    {
        if (clip == null || warningSource == null) return;

        loopSource.clip = clip;
        loopSource.volume = Mathf.Clamp01(volume);
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopLoop()
    {
        loopSource.Stop();
    }

}