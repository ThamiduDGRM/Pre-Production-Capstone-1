using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip musicTrack;
    [Range(0f, 1f)] public float volume = 0.5f;
    public float fadeDuration = 1f;

    private void Awake()
    {
        // Singleton so music persists across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource.loop = true;
        audioSource.volume = 0f; // start silent for fade-in
    }

    private void Start()
    {
        PlayMusic(musicTrack);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicTrack = clip;
        audioSource.clip = musicTrack;
        audioSource.Play();

        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = volume;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}

