using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    [Header("Game Event Sounds")]
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip lifeLostSound;
    [SerializeField] private AudioClip brickDestroyedSound;
    [SerializeField] private AudioClip gameWonSound;

    [Header("Collision Sounds")]
    [SerializeField] private AudioClip paddleHitSound;
    [SerializeField] private AudioClip wallHitSound;

    [Header("Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private string menuSceneName = "Menu";

    private AudioSource sfxSource;
    private AudioSource musicSource;
    private Coroutine musicTransitionCoroutine;

    protected override void Awake()
    {
        base.Awake();

        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Initial music setup based on current scene
        UpdateMusicForCurrentScene();
    }

    private void OnEnable()
    {
        // Re-add the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Use a slight delay to make sure scene is fully loaded
        StartCoroutine(UpdateMusicAfterDelay(0.1f));
    }

    private IEnumerator UpdateMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateMusicForCurrentScene();
    }

    private void UpdateMusicForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string menuScene = menuSceneName;

        if (SceneHandler.Instance != null)
            menuScene = SceneHandler.Instance.GetMenuSceneName();

        Debug.Log($"Scene changed to: {currentScene}, menu scene is: {menuScene}");

        if (currentScene == menuScene)
            PlayMenuMusic();
        else
            PlayGameMusic();
    }

    // SFX methods
    public void PlayGameOverSound() => sfxSource.PlayOneShot(gameOverSound);
    public void PlayLifeLostSound() => sfxSource.PlayOneShot(lifeLostSound);
    public void PlayBrickDestroyedSound() => sfxSource.PlayOneShot(brickDestroyedSound);
    public void PlayPaddleHitSound() => sfxSource.PlayOneShot(paddleHitSound);
    public void PlayWallHitSound() => sfxSource.PlayOneShot(wallHitSound);
    public void PlayGameWonSound() => sfxSource.PlayOneShot(gameWonSound);

    // Music methods
    public void PlayMenuMusic(bool withTransition = true)
    {
        Debug.Log("Playing menu music");
        TransitionToMusic(menuMusic, withTransition);
    }

    public void PlayGameMusic(bool withTransition = true)
    {
        Debug.Log("Playing game music");
        TransitionToMusic(gameMusic, withTransition);
    }

    // This is unnecessary and complicated but I felt like doing it
    public void TransitionToMusic(AudioClip newMusic, bool withTransition = true)
    {
        if (newMusic == null)
        {
            Debug.LogWarning("Trying to play null music clip");
            return;
        }

        // If already playing this music, don't do anything
        if (musicSource.clip == newMusic && musicSource.isPlaying)
        {
            Debug.Log("Already playing requested music");
            return;
        }

        // Stop existing transition
        if (musicTransitionCoroutine != null)
            StopCoroutine(musicTransitionCoroutine);

        float duration = 1.0f;
        if (SceneHandler.Instance != null)
            duration = SceneHandler.Instance.GetAnimationDuration();

        if (withTransition)
            musicTransitionCoroutine = StartCoroutine(CrossFadeMusic(newMusic, duration));
        else
        {
            musicSource.clip = newMusic;
            musicSource.Play();
        }
    }

    private IEnumerator CrossFadeMusic(AudioClip newMusic, float duration)
    {
        float currentTime = 0;
        float startVolume = musicSource.volume;

        // Fade out current music
        if (musicSource.isPlaying)
        {
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
                yield return null;
            }
        }

        // Switch to new clip and fade in
        musicSource.clip = newMusic;
        musicSource.Play();
        musicSource.volume = 0;

        currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, 1, currentTime / duration);
            yield return null;
        }
    }
}