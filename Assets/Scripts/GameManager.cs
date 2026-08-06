using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killText;

    private float timer = 0f;
    private int kills = 0;
    private bool isRunning = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Timer should be visible immediately
        timerText.text = "00:00";        
    }

    private void Update()
    {
        if (!isRunning) return;

        // Wait until countdown is done
        if (!LevelCountdown.Instance.countdownFinished)
            return;

        timer += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void AddKill()
    {
        kills++;
        killText.text = "Kills: " + kills;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}


