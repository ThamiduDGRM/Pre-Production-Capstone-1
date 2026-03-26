using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false); // Hide at start
    }

    public void ShowGameOver()
    {
        if (BackgroundMusic.Instance != null)
            BackgroundMusic.Instance.StopMusic();

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // pause game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        if (BackgroundMusic.Instance != null)
        BackgroundMusic.Instance.PlayMusic(BackgroundMusic.Instance.musicTrack);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit! (Won't close in editor)");
    }
}


