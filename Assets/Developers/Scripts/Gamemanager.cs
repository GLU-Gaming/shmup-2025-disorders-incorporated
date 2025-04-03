using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Gamemanager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public GameObject Player;

    public float maxForceCharge = 300;
    public Image ForceFill;

    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public TMP_Text waveText;

    private int score;
    private int highscore;
    private int waveCount;

    public List<WaveConfiguration> waveConfigurations;
    public float enemyDetectionRadius = 10f; // Radius to check for enemies

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
            yield return new WaitForSeconds(5); // Wait for 5 seconds before checking for the next wave

            // Check if there are no enemies within the detection radius
            if (!AreEnemiesInRange())
            {
                waveCount++;
                UpdateWaveUI();
                if (waveCount <= waveConfigurations.Count)
                {
                    StartCoroutine(SpawnEnemies(waveConfigurations[waveCount - 1]));
                }
                else
                {
                    Debug.LogWarning("No more wave configurations available.");
                }
            }
        }
    }

    private bool AreEnemiesInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(Player.transform.position, enemyDetectionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator SpawnEnemies(WaveConfiguration waveConfig)
    {
        foreach (var config in waveConfig.enemyConfigurations)
        {
            for (int i = 0; i < config.spawnAmount; i++)
            {
                Vector3 spawnPosition = Player.transform.position + Player.transform.forward * config.spawnDistance;
                spawnPosition.y = Random.Range(config.minY, config.maxY);

                Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);

                Instantiate(config.enemyPrefab, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(config.spawnDelay); // Wait for the specified delay before spawning the next enemy
            }
        }
    }
}
