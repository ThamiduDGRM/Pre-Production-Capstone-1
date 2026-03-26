using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int Heal = 10;
    public float coinMultiplier = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyPowerUp(PowerUp p)
    {
        if (p == null)
        {
            Debug.LogError("PowerUp is NULL! Card did not receive a power-up.");
            return;
        }

        switch (p.type)
        {
            case PowerUp.PowerUpType.CoinMultiplier:
                coinMultiplier *= p.value;
                break;

            case PowerUp.PowerUpType.Heal:
                PlayerHealth.Instance.Heal((int)p.value);                              
                break;
          
        }

        Debug.Log("Applied power-up: " + p.powerUpName);
    }
}


