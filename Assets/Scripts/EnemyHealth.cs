using UnityEngine;
using System;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private AudioSource punchAudio;
    public Action onDeath;
    
    [Header("Coin Drop")]
    public GameObject coinPrefab;
    public int coinsToDrop = 1;

    private SpriteRenderer enemyRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flashMaterial;
    private Color originalColor;
    public float flashDuration = 0.4f;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = enemyRenderer.color;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StartCoroutine(FlashRed());
        punchAudio.Play();

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();            
            DropCoins();
            GameManager.Instance.AddKill();
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
        enemyRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.material = defaultMaterial;
        enemyRenderer.color = Color.red;
    }

    private IEnumerator Die()
    {
        float fadeTime = 0.3f;
        float t = 0f;

        Color c = enemyRenderer.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            enemyRenderer.color = c;
            yield return null;
        }

        Destroy(gameObject);
    }
}










