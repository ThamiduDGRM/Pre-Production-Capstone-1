using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonDelay : MonoBehaviour
{
    public Button playAgainButton;
    public Button exitButton;
    public float delay = 0.8f;

    private void OnEnable()
    {
        // Reset buttons every time panel becomes active
        StartCoroutine(EnableButtons());
    }

    private IEnumerator EnableButtons()
    {
        // Safety: disable immediately
        playAgainButton.interactable = false;
        exitButton.interactable = false;

        yield return new WaitForSecondsRealtime(delay);

        playAgainButton.interactable = true;
        exitButton.interactable = true;
    }
}


