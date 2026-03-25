using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3;
    private int currentHealth;

    public Action onDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}




