using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float stopDistance = 6f;
    public float shootInterval = 1.5f;

    public GameObject projectilePrefab;   // PixelBullet / EnemyBullet prefab
    public Animator animator;

    private Transform player;
    private float shootTimer = 0f;
    private Vector3 originalScale;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Prevent movement + shooting until countdown ends
        if (!LevelCountdown.Instance.countdownFinished)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        if (player == null) return;

        FacePlayer();

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            MoveTowardPlayer();
        }
        else
        {
            IdleAndShoot();
        }
    }

    private void MoveTowardPlayer()
    {
        animator.SetBool("isMoving", true);

        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }

    private void IdleAndShoot()
    {
        animator.SetBool("isMoving", false);

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = shootInterval;

            animator.SetTrigger("Shoot");

            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 spawnPos = transform.position + dir * 1.2f;

            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            // Fireball-style bullet movement
            EnemyBullet bullet = proj.GetComponent<EnemyBullet>();
            bullet.direction = dir;
        }
    }

    private void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;

        if (dir.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = originalScale;
    }
}


