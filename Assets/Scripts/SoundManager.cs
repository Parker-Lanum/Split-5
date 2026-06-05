using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources (auto-created if left empty)")]
    public AudioSource warningSource;
    public AudioSource pianoSource;
    public AudioSource sfxSource;
    public AudioSource footstepSource;

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
        footstepSource = EnsureSource(footstepSource);
    }

    AudioSource EnsureSource(AudioSource src)
    {
        if (src == null) src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake  = false;
        src.spatialBlend = 0f;
        return src;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, Mathf.Clamp(volume, 0f, 2f));
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

    public void PlayFootstep(AudioClip clip, float volume = 1f)
    {
        if (clip == null || footstepSource == null) return;

        footstepSource.clip = clip;
        footstepSource.volume = Mathf.Clamp01(volume);
        footstepSource.loop = false;
        footstepSource.Play();
    }

    public void SetWarningVolume(float volume)
    {
        if (warningSource == null) return;
        warningSource.volume = Mathf.Clamp01(volume);
    }

    public void SetPianoVolume(float volume)
    {
        if (pianoSource == null) return;
        pianoSource.volume = Mathf.Clamp01(volume);
    }

    public void StopFootstep()
    {
        if (footstepSource == null) return;
        footstepSource.Stop();
    }

    public bool IsFootstepPlaying()
    {
        return footstepSource != null && footstepSource.isPlaying;
    }

}