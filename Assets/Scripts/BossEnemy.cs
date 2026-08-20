using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    public float attackInterval = 5f;
    public GameObject telegraphPrefab;
    public GameObject arrowPrefab;

    public float telegraphDuration = 1.2f;
    public int arrowsToSpawn = 12;
    public float arrowSpreadRadius = 3f;

    private Transform player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            // Telegraph
            Vector3 telegraphPos = player.position;
            GameObject telegraph = Instantiate(telegraphPrefab, telegraphPos, Quaternion.identity);

            yield return new WaitForSeconds(telegraphDuration);

            // Arrow rain
            for (int i = 0; i < arrowsToSpawn; i++)
            {
                Vector3 randomPos = telegraphPos + Random.insideUnitSphere * arrowSpreadRadius;
                randomPos.y = 10f; // spawn high above

                Instantiate(arrowPrefab, randomPos, Quaternion.identity);
            }

            Destroy(telegraph);
        }
    }
}

