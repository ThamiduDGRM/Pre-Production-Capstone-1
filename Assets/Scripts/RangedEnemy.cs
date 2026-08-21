using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float stopDistance = 6f;       // enemy stops and shoots
    public float shootInterval = 1.5f;    // time between shots
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

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
        if (player == null) return;

        FacePlayer();

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            MoveTowardPlayer();
        }
        else
        {
            ShootAtPlayer();
        }
    }

    private void MoveTowardPlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }

    private void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;

        if (dir.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = originalScale;
    }

    private void ShootAtPlayer()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = shootInterval;

            Vector3 dir = (player.position - transform.position).normalized;

            // Spawn projectile slightly in front
            Vector3 spawnPos = transform.position + dir * 1.2f;

            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            proj.transform.forward = dir;

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.linearVelocity = dir * projectileSpeed;
        }
    }
}
