using UnityEngine;

public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    [Header("Game Event Sounds")]
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip gameWinSound;
    [SerializeField] private AudioClip lifeLostSound;
    [SerializeField] private AudioClip brickDestroyedSound;

    [Header("Collision Sounds")]
    [SerializeField] private AudioClip paddleHitSound;
    [SerializeField] private AudioClip wallHitSound;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.5f;

    private void Start()
    {
        // Set initial volumes
        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;

        // Loop music (edit to end if game over)
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void PlayGameOverSound()
    { //ToDO
        if (gameOverSound != null)
            sfxSource.PlayOneShot(gameOverSound);
    }

    public void PlayGameWinSound()
    {
        if (gameWinSound != null)
            sfxSource.PlayOneShot(gameWinSound);
    }

    public void PlayLifeLostSound()
    {
        if (lifeLostSound != null)
            sfxSource.PlayOneShot(lifeLostSound);
    }

    public void PlayBrickDestroyedSound()
    {
        if (brickDestroyedSound != null)
            sfxSource.PlayOneShot(brickDestroyedSound);
    }

    public void PlayPaddleHitSound()
    {
        if (paddleHitSound != null)
            sfxSource.PlayOneShot(paddleHitSound);
    }

    public void PlayWallHitSound()
    {
        if (wallHitSound != null)
            sfxSource.PlayOneShot(wallHitSound);
    }
}