using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Tracks")]
    public AudioClip[] backgroundTracks;
    public bool shuffleMode = false;

    [Header("UI Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("SFX Clips")]
    public AudioClip clickSFX;
    public AudioClip revealSFX;
    public AudioClip explosionSFX;

    private int lastTrackIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Load saved volumes
        LoadVolumes();

        // Setup sliders
        if (musicSlider != null)
        {
            musicSlider.value = musicSource.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxSource.volume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // -------------------- MUSIC --------------------

    public void PlayMusicForLevel(int level)
    {
        if (backgroundTracks.Length == 0)
            return;

        int index;

        if (shuffleMode)
            index = GetRandomTrackIndex();
        else
            index = (level - 1) % backgroundTracks.Length;

        lastTrackIndex = index;

        musicSource.clip = backgroundTracks[index];
        musicSource.loop = true;
        musicSource.Play();
    }

    private int GetRandomTrackIndex()
    {
        if (backgroundTracks.Length <= 1)
            return 0;

        int index;
        do
        {
            index = Random.Range(0, backgroundTracks.Length);
        }
        while (index == lastTrackIndex);

        return index;
    }

    // -------------------- SFX --------------------

    public void PlayClick() => PlaySFX(clickSFX);
    public void PlayReveal() => PlaySFX(revealSFX);
    public void PlayExplosion() => PlaySFX(explosionSFX);

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    // -------------------- VOLUME --------------------

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void LoadVolumes()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSource.volume = musicVol;
        sfxSource.volume = sfxVol;

        if (musicSlider != null)
            musicSlider.value = musicVol;

        if (sfxSlider != null)
            sfxSlider.value = sfxVol;
    }
}
