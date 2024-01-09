using UnityEngine;
using TMPro;

public class TimeManagement : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text scoreText;

    private float startTime;
    private float endTime;
    private float taskDuration;
    private float score;

    public ScoreScreen scoreScreen;

    void Start()
    {
        StartTask();
    }

    void Update()
    {
        UpdateTime();
    }

    public void StartTask()
    {
        startTime = Time.time;
        score = 0;
        DisplayScore();
    }

    void UpdateTime()
    {
        float currentTime = Time.time - startTime;
        timeText.text = "Time Elapsed: " + currentTime.ToString("F2") + "s";
    }

    public void EndTask()  // Change the access modifier to public
    {
        endTime = Time.time;
        taskDuration = endTime - startTime;

        EvaluatePerformance(taskDuration);

        // Show the score screen
        scoreScreen.ShowScore(score);
    }

    void EvaluatePerformance(float duration)
    {
        // Performance evaluation based on duration
        if (duration < 10f)
        {
            Debug.Log("Excellent! Task completed very quickly.");
            score = 100f;
        }
        else
        {
            score = (100f - duration - 10f) / 100f;
        }

        DisplayScore();
    }

    void DisplayScore()
    {
        scoreText.text = "Score: " + score.ToString("F2");
    }
}
