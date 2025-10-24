using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 60f;
    private bool isTiming = false;
    private bool levelFinished = false;

    void Start()
    {
        UpdateTimerUI();   
    }

    void Update()
    {
        if (isTiming && !levelFinished)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isTiming = false;
                FailLevel();
            }

            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        isTiming = true;
    }

    public void FinishLevel()
    {
        levelFinished = true;
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void FailLevel()
    {
        timerText.text = "You Fail";
    }
}
