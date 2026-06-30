using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject instructionsPanel;

    private void Start()
    {
        // Pause the game at the beginning
        Time.timeScale = 0f;
        startPanel.SetActive(true);
        instructionsPanel.SetActive(true);
    }

    public void StartGame()
    {
        // Unpause the game
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }
}

