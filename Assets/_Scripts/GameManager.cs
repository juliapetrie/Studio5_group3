using UnityEngine;
using System.Collections;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private LiveCounterUI liveCounterUI;
    [SerializeField] private int score;
    [SerializeField] private ScoreCounterUI scoreCounter;

    public GameObject gameOverUI;

    private int currentBrickCount;
    private int totalBrickCount;
    private ParticleSystem damageParticleInstance; 

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here     
        
        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
         //Camera Shake
        CameraShake.Shake(0.2f, 0.1f);
        if(currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
        //particle system
        SpawnDamageParticles(position);
    }

    private void SpawnDamageParticles(Vector3 spawnPosition)
    {
            damageParticleInstance = Instantiate(damageParticles, spawnPosition, Quaternion.identity);

            // damageParticleInstance.Play();
    }
    
    public void KillBall()
    {
        maxLives--;
        liveCounterUI.UpdateLives(maxLives);
        // update lives on HUD here
        // game over UI if maxLives < 0, then exit to main menu after delay
        if (maxLives < 0)
        {
            StartCoroutine(GameOverSequence());
        }
        else
        {
            ball.ResetBall();
        }
    }

    private IEnumerator GameOverSequence()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);
        SceneHandler.Instance.LoadMenuScene();

        gameOverUI.SetActive(false);
        
        SceneHandler.Instance.LoadMenuScene();
        Time.timeScale = 1f;
    }

    public void IncreaseScore()
    {
        score++;
        scoreCounter.UpdateScore(score);
    }
}
