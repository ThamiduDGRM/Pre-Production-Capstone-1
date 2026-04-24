// PlayerStats.cs (updated)
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int Heal = 10;
    public float coinMultiplier = 1f;

    [Header("Bomb Settings")]
    public int bombCount = 0;
    public GameObject bombPrefab; // assign prefab in inspector
    public Transform bombSpawnPoint; // optional: where bombs spawn (child of player)

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

            case PowerUp.PowerUpType.Bomb:
                // value used as number of bombs to add
                bombCount += Mathf.Max(1, Mathf.RoundToInt(p.value));
                break;
        }

        Debug.Log("Applied power-up: " + p.powerUpName);
    }

    // Call this to drop a bomb. Returns true if a bomb was dropped.
    public bool DropBomb()
    {
        if (bombCount <= 0)
        {
            Debug.Log("No bombs available");
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

        Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        bombCount--;
        Debug.Log("Bomb dropped. Remaining bombs: " + bombCount);
        return true;
    }
}



