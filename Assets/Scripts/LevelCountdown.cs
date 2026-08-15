using UnityEngine;
using TMPro;
using System.Collections;
using System; //Added so we can use Action events

public class LevelCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float countdownSpeed = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioSource announcerAudio;

    public bool countdownFinished { get; private set; } = false;
    public static LevelCountdown Instance;

    //Event broadcast when the intro countdown completes
    public static event Action OnCountdownFinished;

    private void Awake()
    {
        Instance = this;
    }

    //LevelFadeIn will call this
    public void BeginCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        countdownFinished = false;        
        countdownText.gameObject.SetActive(true);

        //Null check safety for audio
        if (announcerAudio != null)
        {
            announcerAudio.Play();
        }

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

        //Mark finished and trigger event for Level2Countdown
        countdownFinished = true;
        OnCountdownFinished?.Invoke();
    }
}