using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 10;
    private int currentHealth;

    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: Game over, respawn, etc.
    }
}

