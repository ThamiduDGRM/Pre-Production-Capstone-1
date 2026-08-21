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

    [Header("Point Attack")]
    public GameObject pointProjectilePrefab;
    public float projectileSpeed = 12f;
    public int projectilesToShoot = 3;
    public float projectileInterval = 0.2f;

           
    

    private Transform player;
    private bool isAttacking = false;
    private Vector3 originalScale;

    private bool usePointAttack = false; // alternates between attacks

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
        if (!LevelCountdown.Instance.countdownFinished)
        {
            bossAnimator.SetBool("isMoving", false);
            return;
        }

        if (isAttacking)
        {
            bossAnimator.SetBool("isMoving", false);
            return;
        }

        if (player == null) return;

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        bossAnimator.SetBool("isMoving", true);

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

            Vector3 telegraphPos = player.position;
            GameObject telegraph = Instantiate(telegraphPrefab, telegraphPos, Quaternion.identity);

            if (usePointAttack)
            {
                bossAnimator.SetTrigger("TelegraphPoint");
                yield return new WaitForSeconds(telegraphDuration);
                yield return StartCoroutine(PointAttack(player.position));
            }
            else
            {
                bossAnimator.SetTrigger("Telegraph");
                yield return new WaitForSeconds(telegraphDuration);

                for (int i = 0; i < arrowsToSpawn; i++)
                {
                    Vector3 randomPos = telegraphPos + Random.insideUnitSphere * arrowSpreadRadius;
                    randomPos.y = 10f;

                    Instantiate(arrowPrefab, randomPos, Quaternion.identity);
                }

                yield return new WaitForSeconds(0.5f);
            }

            Destroy(telegraph);

            usePointAttack = !usePointAttack;

            isAttacking = false;
        }
    }

    private IEnumerator PointAttack(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;

        for (int i = 0; i < projectilesToShoot; i++)
        {
            Vector3 spawnPos = transform.position + (dir * 1.2f);

            GameObject proj = Instantiate(pointProjectilePrefab, spawnPos, Quaternion.identity);

            proj.transform.forward = dir;
            proj.transform.Rotate(90f, 0f, 0f);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.linearVelocity = dir * projectileSpeed;
            }

            yield return new WaitForSeconds(projectileInterval);
        }
    }

    // ⭐ NEW — Boss drops a coin every time he is punched
   
}





