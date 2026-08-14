using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private int killsRequired = 40;
    private int currentKills = 0;

    [SerializeField] private GameObject levelCompleteText;

    [SerializeField] private GameObject levelCompleteUI;   // ADD THIS

    [SerializeField] private UnityEngine.UI.Button playAgainButton;
    [SerializeField] private UnityEngine.UI.Button exitButton;


    private void Awake()
    {
        Instance = this;

        if (levelCompleteText != null)
            levelCompleteText.SetActive(false);
        
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
   
    }

    public void RegisterKill()
    {
        currentKills++;

        if (currentKills >= killsRequired)
            OnLevelComplete();
    }

    private void OnLevelComplete()
    {
        if (levelCompleteText != null)
            levelCompleteText.SetActive(true);
        StartCoroutine(EnableButtonsAfterDelay());
        // SHOW EXIT + RESTART BUTTONS
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(true);
        
        if (BackgroundMusic.Instance != null)
            BackgroundMusic.Instance.StopMusic();
        
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
            player.DisableInput();
        Time.timeScale = 0f;

        Debug.Log("LEVEL 1 COMPLETE!");
    }


    private IEnumerator EnableButtonsAfterDelay()
    {
      playAgainButton.interactable = false;
      exitButton.interactable = false;

      yield return new WaitForSecondsRealtime(0.8f);

      playAgainButton.interactable = true;
      exitButton.interactable = true;
    }

}



