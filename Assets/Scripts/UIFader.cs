using UnityEngine;
using System.Collections;

public class UIFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private float fadeDuration = 0.4f;
    public System.Action onFadeComplete;

    private void Awake()
    {
        if (group == null)
            group = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeRoutine(1f, 0f));
    }

    private IEnumerator FadeRoutine(float start, float end)
    {
        float t = 0f;
        group.alpha = start;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // IMPORTANT: works while Time.timeScale = 0
            group.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        group.alpha = end;

        group.interactable = end == 1f;
        group.blocksRaycasts = end == 1f;

        if (end == 1f)
            onFadeComplete?.Invoke();
        
        if (end == 0f)
            gameObject.SetActive(false);
    }
}


