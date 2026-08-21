using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    [SerializeField] private UnityEngine.UI.Button nextLevelButton;

    private void Awake()
    {
        Instance = this;

        if (levelCompleteText != null)
            levelCompleteText.SetActive(false);

        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);

        if (nextLevelButton != null)
            nextLevelButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        // LEVEL 2 SURVIVAL MODE
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

    //  LEVEL 1 KILL COUNTER
    public void RegisterKill()
    {
        if (!level2Active)
        {
            currentKills++;

            if (currentKills >= killsRequired)
                OnLevelComplete();
        }
    }

    public void OnLevelComplete()
    {
        if (levelCompleteText != null)
            levelCompleteText.SetActive(true);

        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(true);

        //  Show Next Level button
        if (nextLevelButton != null)
            nextLevelButton.gameObject.SetActive(true);

        if (BackgroundMusic.Instance != null)
            BackgroundMusic.Instance.StopMusic();

        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
            player.DisableInput();

        // Disable enemies
            foreach (EnemyAi ai in Object.FindObjectsByType<EnemyAi>(FindObjectsSortMode.None))
        {
            Destroy(ai.gameObject);
        }

        foreach (EnemyAi2 ai2 in Object.FindObjectsByType<EnemyAi2>(FindObjectsSortMode.None))
        {
            Destroy(ai2.gameObject);
        }

        foreach (RangedEnemy re in Object.FindObjectsByType<RangedEnemy>(FindObjectsSortMode.None))
        {
            Destroy(re.gameObject);
        }

       
        

        GameManager gamemanager = Object.FindFirstObjectByType<GameManager>();
        if (gamemanager != null)
            gamemanager.gameObject.SetActive(false);

        Time.timeScale = 1f;

        StartCoroutine(EnableButtonsAfterDelay());

        Debug.Log("LEVEL COMPLETE!");
    }

    private IEnumerator EnableButtonsAfterDelay()
    {
        playAgainButton.interactable = false;
        exitButton.interactable = false;
        nextLevelButton.interactable = false;

        yield return new WaitForSecondsRealtime(0.8f);

        playAgainButton.interactable = true;
        exitButton.interactable = true;
        nextLevelButton.interactable = true;
    }

    //  LEVEL 2 START
    public void StartLevel2()
    {
        level2Active = true;
        survivalTime = 120f;
        currentKills = 0;
    }

    //  NEXT LEVEL BUTTON LOGIC
    public void NextLevel()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}






