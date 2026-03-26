using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    
    public int maxHealth = 10;
    public float coinMultiplier = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyPowerUp(PowerUp p)
    {
        switch (p.type)
        {            
            case PowerUp.PowerUpType.CoinMultiplier:
                coinMultiplier += p.value;
                break;

            case PowerUp.PowerUpType.MaxHealth:
                maxHealth += (int)p.value;
                break;
        }
    }
}

