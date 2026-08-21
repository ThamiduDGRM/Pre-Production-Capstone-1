using UnityEngine;
using TMPro;
using System.Collections;

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

    private IEnumerator CountdownRoutine()
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

        

        countdownText.gameObject.SetActive(false);

        countdownFinished = true;
    }
}



