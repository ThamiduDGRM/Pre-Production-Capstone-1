using UnityEngine;
using System;
using System.Collections;

public class EnemyHealth2 : MonoBehaviour, IDamageable
{
    public int maxHealth = 3;
    private int currentHealth;

    public Action onDeath;

    [Header("Coin Drop")]
    public GameObject coinPrefab;
    public int coinsToDrop = 1;

    private Renderer enemyRenderer;
    private Color originalColor;
    public float flashDuration = 0.1f;

    public AudioSource kickAudio;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponentInChildren<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StartCoroutine(FlashRed());
        kickAudio.Play();

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            DropCoins();
            GameManager.Instance.AddKill();
            LevelManager.Instance.RegisterKill();
            StartCoroutine(Die());
        }
    }

    private void DropCoins()
    {
        for (int i = 0; i < coinsToDrop; i++)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator FlashRed()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.material.color = originalColor;
    }

    private IEnumerator Die()
    {
        float fadeTime = 0.3f;
        float t = 0f;

        Color c = enemyRenderer.material.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            enemyRenderer.material.color = c;
            yield return null;
        }

        Destroy(gameObject);
    }
}
