using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static PlayerHealth Instance;

    public int maxHealth = 30;
    public int currentHealth;

    public Slider healthBar;
    private Renderer playerRenderer;
    private Color originalColor;
    public float flashDuration = 0.1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        playerRenderer = GetComponentInChildren<Renderer>();
        originalColor = playerRenderer.material.color;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;
        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            Die();
        }
    }
     private IEnumerator FlashRed()
    {
        playerRenderer.material.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        playerRenderer.material.color = originalColor;
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;
    }

    private void Die()
    {
        Debug.Log("PLAYER DIED");
        GameOverUI ui = FindFirstObjectByType<GameOverUI>();
        ui?.ShowGameOver();
    }
}







