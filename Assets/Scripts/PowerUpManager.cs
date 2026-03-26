using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    public PowerUp[] allPowerUps;
    public GameObject powerUpUI;

    public int coinThreshold = 20;
    private int nextThreshold;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        nextThreshold = coinThreshold;
        powerUpUI.SetActive(false);
    }

    public void OnCoinsChanged(int coins)
    {
        if (coins >= nextThreshold)
        {
            nextThreshold += coinThreshold;
            ShowPowerUpSelection();
        }
    }

    public void ShowPowerUpSelection()
    {
        Time.timeScale = 0f; // pause game
        powerUpUI.SetActive(true);

        PowerUpUI.Instance.DisplayRandomCards(allPowerUps);
    }

    public void ApplyPowerUp(PowerUp p)
    {
        PlayerStats.Instance.ApplyPowerUp(p);

        powerUpUI.SetActive(false);
        Time.timeScale = 1f; // resume game
    }
}

