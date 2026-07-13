using UnityEngine;
using System.Collections;

public class LevelFadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] private LevelCountdown countdown;

    private void Start()
    {
        countdown.BeginCountdown();
        StartCoroutine(FadeInRoutine());        
    }

    private IEnumerator FadeInRoutine()
    {
        fadeGroup.alpha = 1f;

        float t = 0f;        
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        fadeGroup.alpha = 0f;
        fadeGroup.gameObject.SetActive(false);

        // Start countdown AFTER fade
        
    }
}

