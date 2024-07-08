using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";

    void Awake()
    {
        // Singleton pattern to ensure only one instance of AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load saved volume settings
        var ret = LoadVolumeSettings();

        SetMusicVolume(ret.musicVolume);
        SetSfxVolume(ret.sfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;

        // Play or stop music based on volume
        if (volume > 0.001 && !musicSource.isPlaying)
        {
            musicSource.loop = true;
            musicSource.Play();
        }
        else if (volume < 0.001 && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void PlaySfx(AudioClip clip)
    {
        if (sfxSource.volume > 0.001)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public static (float musicVolume, float sfxVolume) LoadVolumeSettings()
    {
        float musicVolume = 1.0f, sfxVolume = 1.0f;

        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
        }

        if (PlayerPrefs.HasKey(SfxVolumeKey))
        {
            sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey);
        }

        return (musicVolume, sfxVolume);
    }

    public static void SaveVolumeSettings(float musicVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolume);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(musicVolume);
            AudioManager.Instance.SetSfxVolume(sfxVolume);
        }
    }
}
