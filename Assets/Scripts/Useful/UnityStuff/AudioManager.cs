using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public AudioSource musicSource;
    public AudioMixer audioMixer;
    
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string MUSIC_ENABLED_KEY = "MusicEnabled";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeAudio()
    {
        float savedVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.75f);
        int musicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1);
        
        SetVolume(savedVolume);
        SetMusicEnabled(musicEnabled == 1);
        
        if (!musicSource.isPlaying) musicSource.Play();
    }

    public void SetVolume(float volume)
    {
        float dbVolume = volume > 0.01f ? 20f * Mathf.Log10(volume) : -80f;
        
        audioMixer.SetFloat("MusicVolume", dbVolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }
    
    public void SetMusicEnabled(bool enabled)
    {
        musicSource.mute = !enabled;
        PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, enabled ? 1 : 0);
    }

    public float GetCurrentVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.75f);
    }

    public bool IsMusicEnabled()
    {
        return PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1;
    }
}