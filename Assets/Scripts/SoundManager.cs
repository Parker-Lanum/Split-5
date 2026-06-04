using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources (auto-created if left empty)")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        musicSource = EnsureSource(musicSource);
        sfxSource   = EnsureSource(sfxSource);
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

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = volume;
        musicSource.Play();
    }
}