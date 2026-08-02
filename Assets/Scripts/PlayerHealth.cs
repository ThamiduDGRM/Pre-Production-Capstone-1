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

    private SpriteRenderer playerRenderer;   // FIXED: use SpriteRenderer
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

        playerRenderer = GetComponentInChildren<SpriteRenderer>();   // FIXED
        originalColor = playerRenderer.color;              // FIXED
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
        playerRenderer.color = Color.red;                 // FIXED
        yield return new WaitForSeconds(flashDuration);
        playerRenderer.color = originalColor;             // FIXED
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
        GameManager.Instance.StopTimer();
        GameOverUI ui = FindFirstObjectByType<GameOverUI>();
        ui?.ShowGameOver();
    }
}








