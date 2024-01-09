using UnityEngine;
using TMPro;

public class ScoreScreen : MonoBehaviour
{
    public TMP_Text scoreText;

    // Display the score on the screen
    public void ShowScore(float score)
    {
        scoreText.text = "Your Score: " + score.ToString("F2");
        gameObject.SetActive(true); // Activate the panel
    }

    // Hide the score screen
    public void HideScore()
    {
        gameObject.SetActive(false); // Deactivate the panel
    }
}
