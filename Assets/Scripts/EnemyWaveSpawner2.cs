using UnityEngine;
using System.Collections;

public class EnemyWaveSpawner2 : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;   // time between spawns
    public int maxEnemies = 10;        // limit on-screen enemies

    private int enemiesAlive = 0;

    private void Start()
    {
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

        EnemyHealth2 health = enemy.GetComponent<EnemyHealth2>();
        health.onDeath += OnEnemyDeath;

        enemiesAlive++;
    }

    private void OnEnemyDeath()
    {
        enemiesAlive--;
    }
}
