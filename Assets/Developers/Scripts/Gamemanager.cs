using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gamemanager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public GameObject Player;

    public float maxForceCharge = 300;
    public Image ForceFill;

    public Text scoreText;
    public Text highscoreText;
    public Text waveText;

    private int score;
    private int highscore;
    private int waveCount;

    public GameObject enemyPrefab;
    public float spawnDistance = 10f; // Distance in front of the player to spawn enemies
    public float minY = -11f; // Minimum Y value for spawning
    public float maxY = 8f; // Maximum Y value for spawning

    private void Start()
    {
        playerMovement = Player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on the player.");
        }

        UpdateForceChargeUI(maxForceCharge); // Initialize the UI bar to full charge

        score = 0;
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        waveCount = 0;

        UpdateScoreUI();
        UpdateHighscoreUI();
        UpdateWaveUI();

        StartCoroutine(GameLoop());
    }

    public void UpdateForceChargeUI(float currentCharge)
    {
        if (ForceFill != null)
        {
            ForceFill.fillAmount = currentCharge / maxForceCharge;
            Debug.Log("Force Charge Updated: " + ForceFill.fillAmount);
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
            UpdateHighscoreUI();
        }
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void UpdateHighscoreUI()
    {
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + highscore.ToString();
        }
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + waveCount.ToString();
        }
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5); // Wait for 5 seconds before starting the next wave
            waveCount++;
            UpdateWaveUI();
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < 5; i++) // Spawn 5 enemies per wave
        {
            Vector3 spawnPosition = Player.transform.position + Player.transform.forward * spawnDistance;
            spawnPosition.y = Random.Range(minY, maxY);

            // Create a rotation with 90 degrees on the X and Y axes
            Quaternion spawnRotation = Quaternion.Euler(0, -90, 0);

            Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(1); // Wait for 1 second before spawning the next enemy
        }
    }
}
