using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level 1 Settings")]
    [SerializeField] private int killsRequired = 40;
    private int currentKills = 0;

    [Header("Level 2 Settings")]
    public bool level2Active = false;
    public float survivalTime = 120f; // 2 minutes

    [Header("UI")]
    [SerializeField] private GameObject levelCompleteText;
    [SerializeField] private GameObject levelCompleteUI;
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

    private void Update()
    {
        // ⭐ LEVEL 2 SURVIVAL MODE
        if (level2Active)
        {
            survivalTime -= Time.deltaTime;

            if (survivalTime <= 0f)
            {
                survivalTime = 0f;
                OnLevelComplete();
            }
        }
    }

    // ⭐ LEVEL 1 KILL COUNTER (unchanged)
    public void RegisterKill()
    {
        if (!level2Active) // Only count kills in Level 1
        {
            currentKills++;

            if (currentKills >= killsRequired)
                OnLevelComplete();
        }
    }

    private void OnLevelComplete()
    {
        if (levelCompleteText != null)
            levelCompleteText.SetActive(true);

        StartCoroutine(EnableButtonsAfterDelay());

        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(true);

        if (BackgroundMusic.Instance != null)
            BackgroundMusic.Instance.StopMusic();

        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
            player.DisableInput();

        Time.timeScale = 0f;

        Debug.Log("LEVEL COMPLETE!");
    }

    private IEnumerator EnableButtonsAfterDelay()
    {
        playAgainButton.interactable = false;
        exitButton.interactable = false;

        yield return new WaitForSecondsRealtime(0.8f);

        playAgainButton.interactable = true;
        exitButton.interactable = true;
    }

    // ⭐ Call this when Level 2 starts
    public void StartLevel2()
    {
        level2Active = true;
        survivalTime = 120f; // reset timer
        currentKills = 0;    // kill count irrelevant for Level 2
    }
}




