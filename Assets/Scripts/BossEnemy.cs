using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    public float attackInterval = 5f;
    public float moveSpeed = 2f;

    [Header("Telegraph")]
    public GameObject telegraphPrefab;
    public float telegraphDuration = 1.2f;
    public Animator bossAnimator;

    [Header("Arrow Rain")]
    public GameObject arrowPrefab;
    public int arrowsToSpawn = 12;
    public float arrowSpreadRadius = 3f;

    private Transform player;
    private bool isAttacking = false;
    private Vector3 originalScale;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        originalScale = transform.localScale;
        StartCoroutine(AttackRoutine());
    }

    private void FixedUpdate()
    {
        // Block boss movement until countdown finishes
        if (!LevelCountdown.Instance.countdownFinished)
        {
            bossAnimator.SetBool("isMoving", false);
            return;
        }
        if (isAttacking) return;
        if (player == null) return;

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        bossAnimator.SetBool("isMoving", true);

        // Move toward player using EnemyAI-style movement
        Vector3 targetPos = new Vector3
        (
            player.position.x,
            transform.position.y,
            player.position.z
        );

        transform.position = Vector3.MoveTowards
        (
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        FaceTarget(player.position);
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        if (Mathf.Abs(direction.x) < 0.2f)
            return;

        if (direction.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = originalScale;
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            isAttacking = true;
            bossAnimator.SetBool("isMoving", false);

            // Telegraph animation
            bossAnimator.SetTrigger("Telegraph");

            // Spawn telegraph under player
            Vector3 telegraphPos = player.position;
            GameObject telegraph = Instantiate(telegraphPrefab, telegraphPos, Quaternion.identity);

            // Wait for animation duration
            yield return new WaitForSeconds(telegraphDuration);

            // Arrow rain
            for (int i = 0; i < arrowsToSpawn; i++)
            {
                Vector3 randomPos = telegraphPos + Random.insideUnitSphere * arrowSpreadRadius;
                randomPos.y = 10f;

                Instantiate(arrowPrefab, randomPos, Quaternion.identity);
            }

            Destroy(telegraph);

            isAttacking = false;
        }
    }
}



