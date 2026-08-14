// PlayerStats.cs (updated)
using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int Heal = 10;
    public float coinMultiplier = 1f;

    [Header("Bomb Settings")]
    [SerializeField] private int bombCount = 0;
    [SerializeField] private bool bombAbilityUnlocked = false;

    public GameObject bombPrefab;
    public Transform bombSpawnPoint;

    [Header("Fireball Settings")]
    public bool fireballActive = false;
    public float fireballDuration = 5f;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;

    // Public read-only access for other scripts
    public int BombCount => bombCount;
    public bool BombAbilityUnlocked => bombAbilityUnlocked;

    private void Awake()
    {
        Instance = this;

        // Bomb ability must always start locked.
        bombAbilityUnlocked = false;
        bombCount = 0;
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

            case PowerUp.PowerUpType.Bomb:

                // Explicitly unlock the Bomb ability.
                bombAbilityUnlocked = true;

                // Add the number of bombs granted by the card.
                bombCount += Mathf.Max(1, Mathf.RoundToInt(p.value));

                Debug.Log(
                    "Bomb power-up selected! " +
                    "Bomb ability UNLOCKED. Bombs: " + bombCount
                );

                break;

            case PowerUp.PowerUpType.FireballMode:

                StartCoroutine(ActivateFireballMode(p.value));
                break;
        }

        Debug.Log("Applied power-up: " + p.powerUpName);
    }

    private IEnumerator ActivateFireballMode(float duration)
    {
        fireballActive = true;

        yield return new WaitForSeconds(duration);

        fireballActive = false;
    }

    public bool DropBomb()
    {
        // HARD LOCK:
        // The player cannot drop a bomb unless the Bomb power-up
        // has explicitly unlocked the ability.
        if (!bombAbilityUnlocked)
        {
            Debug.Log("Bomb ability is locked. Select the Bomb power-up first.");
            return false;
        }

        // The ability is unlocked, but the player has no bombs left.
        if (bombCount <= 0)
        {
            Debug.Log("Bomb ability unlocked, but no bombs remaining.");
            return false;
        }

        if (bombPrefab == null)
        {
            Debug.LogError("Bomb prefab not assigned in PlayerStats.");
            return false;
        }

        Vector3 spawnPos = transform.position;

        if (bombSpawnPoint != null)
            spawnPos = bombSpawnPoint.position;

        Instantiate(
            bombPrefab,
            spawnPos,
            Quaternion.identity
        );

        bombCount--;

        Debug.Log(
            "Bomb dropped. Remaining bombs: " + bombCount
        );

        return true;
    }
}



