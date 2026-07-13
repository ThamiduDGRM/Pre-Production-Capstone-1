using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public int maxEnemies = 10;

    private int enemiesAlive = 0;

    [Header("Countdown Reference")]
    [SerializeField] private LevelCountdown countdown;   // ← IMPORTANT

    private void Start()
    {
        StartCoroutine(WaitForCountdownThenSpawn());
    }

    private IEnumerator WaitForCountdownThenSpawn()
    {
        // Wait until countdown finishes
        while (!countdown.countdownFinished)
            yield return null;

        // Now begin spawning
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (enemiesAlive < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        health.onDeath += OnEnemyDeath;

        enemiesAlive++;
    }

    private void OnEnemyDeath()
    {
        enemiesAlive--;
    }
}



