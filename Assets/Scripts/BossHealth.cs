using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Coin Drop")]
    public GameObject coinPrefab;
    public Transform coinDropPoint;

    [Header("Hit Feedback")]
    public AudioSource punchAudio;
    public Renderer bossRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private Color originalColor;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (bossRenderer != null)
            originalColor = bossRenderer.material.color;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        DropCoin();
        PlayPunchSound();
        FlashRed();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void DropCoin()
    {
        if (coinPrefab == null) return;

        Vector3 pos = coinDropPoint != null ? coinDropPoint.position : transform.position;
        Instantiate(coinPrefab, pos, Quaternion.identity);
    }

    private void PlayPunchSound()
    {
        if (punchAudio != null)
            punchAudio.Play();
    }

    private void FlashRed()
    {
        if (bossRenderer == null) return;

        bossRenderer.material.color = flashColor;
        Invoke(nameof(ResetColor), flashDuration);
    }

    private void ResetColor()
    {
        if (bossRenderer != null)
            bossRenderer.material.color = originalColor;
    }

    private void Die()
    {
        LevelManager.Instance.OnLevelComplete();
        Destroy(gameObject);
    }
}







