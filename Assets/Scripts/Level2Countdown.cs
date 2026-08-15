using System.Collections;
using UnityEngine;
using TMPro;

public class Level2Countdown : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text timerText;

    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 120f; //2 minutes 

    //controls gameplay active or the level finished
    public bool countdownFinished { get; private set; } = false;

    private bool isTimerRunning = false;

    private void OnEnable()
    {
        //connect to the start countdown event
        LevelCountdown.OnCountdownFinished += StartSurvivalTimer;
    }

    private void OnDisable()
    {
        //disconnect to prevent memory leaks
        LevelCountdown.OnCountdownFinished -= StartSurvivalTimer;
    }

    private void Start()
    {
        //show timer (02:00) on UI while waiting for start countdown
        UpdateTimerDisplay(timeRemaining);
        isTimerRunning = false;

        // Fallback safety check in case the start countdown finished before this script loaded
        if (LevelCountdown.Instance != null && LevelCountdown.Instance.countdownFinished)
        {
            StartSurvivalTimer();
        }
    }

    private void StartSurvivalTimer()
    {
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
            // Timer finished
            timeRemaining = 0;
            isTimerRunning = false;
            countdownFinished = true; // level complete / time up
            UpdateTimerDisplay(0);
            OnSurvivalComplete();
        }
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        // Prevent displaying negatives
        if (timeToDisplay < 0) timeToDisplay = 0;

        // Calculate mins and secs
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Format as 02:00
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void OnSurvivalComplete()
    {
        Debug.Log("Level Survived!");
        // Add level progression logic here
    }
}