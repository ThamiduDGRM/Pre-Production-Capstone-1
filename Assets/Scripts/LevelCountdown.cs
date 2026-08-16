using UnityEngine;
using TMPro;
using System.Collections;
using System; // Required for Action

public class LevelCountdown : MonoBehaviour
{
    public static LevelCountdown Instance;

    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float countdownSpeed = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioSource announcerAudio;

    public bool countdownFinished { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // AUTO-STARTS IN EVERY LEVEL
        StartCoroutine(CountdownRoutine());
    }

    public IEnumerator CountdownRoutine()
    {
        countdownFinished = false;
        countdownText.gameObject.SetActive(true);

        if (announcerAudio != null)
            announcerAudio.Play();

        countdownText.text = "3";
        yield return new WaitForSeconds(countdownSpeed);

        countdownText.text = "2";
        yield return new WaitForSeconds(countdownSpeed);

        countdownText.text = "1";
        yield return new WaitForSeconds(countdownSpeed);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(countdownSpeed);

        countdownText.text = "Kill All Enemies!";
        yield return new WaitForSeconds(countdownSpeed);

        countdownText.gameObject.SetActive(false);

        // Notify Level2Countdown that intro is complete
        countdownFinished = true;
        OnCountdownFinished?.Invoke();
    }
}



