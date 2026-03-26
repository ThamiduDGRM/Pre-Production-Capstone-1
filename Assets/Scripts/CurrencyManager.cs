using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int coins = 0;
    public TextMeshProUGUI coinText; // assign in inspector

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        int finalAmount = Mathf.RoundToInt(amount * PlayerStats.Instance.coinMultiplier);

        coins += finalAmount;
        UpdateUI();

        PowerUpManager.Instance.OnCoinsChanged(coins);
    }


    public void UpdateUI()
    {
        coinText.text = "Coins: " + coins;
    }
}

