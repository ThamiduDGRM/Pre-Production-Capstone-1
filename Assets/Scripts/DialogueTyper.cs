using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueTyper : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private CanvasGroup panelGroup;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Audio")]
    [SerializeField] private AudioSource narrationAudio;

    public void StartDialogue(string text)
    {
        StartCoroutine(TypeRoutine(text));
    }

    private IEnumerator TypeRoutine(string text)
    {
        // Fade in panel
        panelGroup.alpha = 0f;
        panelGroup.gameObject.SetActive(true);

        while (panelGroup.alpha < 1f)
        {
            panelGroup.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        dialogueText.text = "";
        narrationAudio.Play();

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        narrationAudio.Stop();
    }
}

