using UnityEngine;
using TMPro;

public class WinScreenManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highscoreText;

    private void Start()
    {
        // Retrieve the score and highscore from PlayerPrefs
        int score = PlayerPrefs.GetInt("Score", 0);
        int highscore = PlayerPrefs.GetInt("Highscore", 0);

        // Update the UI with the retrieved scores
        UpdateScoreUI(score);
        UpdateHighscoreUI(highscore);
    }

    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void UpdateHighscoreUI(int highscore)
    {
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + highscore.ToString();
        }
    }
}
