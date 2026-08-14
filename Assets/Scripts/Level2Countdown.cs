using System.Collections;
using UnityEngine;
using TMPro; //switch to 'using UnityEngine.UI;' if using Legacy UI Text

public class LevelCountdown : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text timerText; //drag UI Text object here

    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 120f; //2 minutes (120seconds) 

    //controls  gameplay active or the level finished
    public bool countdownFinished { get; private set; } = false;

    private bool isTimerRunning = false;

    private void Start()
    {
        //start the timer immediately when the scene loads
        isTimerRunning = true;
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay(timeRemaining);
        }
        else
        {
            //Timer up!
            timeRemaining = 0;
            isTimerRunning = false;
            countdownFinished = true; //level complete / time up
            UpdateTimerDisplay(0);
            OnSurvivalComplete();
        }
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        //prevent display from showing negatives
        if (timeToDisplay < 0) timeToDisplay = 0;

        //calculate mins and secs
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        //format as 02:00
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnSurvivalComplete()
    {
        Debug.Log("Level Survived!");
        //add level progression logic
    }
}