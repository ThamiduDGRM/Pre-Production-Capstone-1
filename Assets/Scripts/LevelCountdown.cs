using UnityEngine;
using TMPro;
using System.Collections;

public class LevelCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float countdownSpeed = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioSource announcerAudio;

    public bool countdownFinished { get; private set; } = false;

    // LevelFadeIn will call this
    public void BeginCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        announcerAudio.Play();
        countdownFinished = false;        
        countdownText.gameObject.SetActive(true);

        // Play announcer voice
        

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


