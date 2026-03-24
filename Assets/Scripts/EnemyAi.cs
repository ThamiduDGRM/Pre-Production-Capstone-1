using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 6f;
    public float attackRange = 1.5f;
    public Transform[] patrolPoints;
    public Animator animator;
    private Vector3 originalScale;

    private int currentPatrolIndex = 0;
    private Transform player;
    private bool isAttacking = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalScale = transform.localScale;

    }

    public void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (isAttacking) return;

        animator.SetBool("IsMoving", true);

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            patrolSpeed * Time.deltaTime
        );

        // Switch to next patrol point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.6f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

        FaceTarget(targetPoint.position);
    }

    private void ChasePlayer()
    {
        if (isAttacking) return;

        animator.SetBool("IsMoving", true);

        Vector3 targetPos = new Vector3(
        player.position.x,
        transform.position.y,   // keep enemy on the ground
        player.position.z
    );

        transform.position = Vector3.MoveTowards(
        transform.position,
        targetPos,
        chaseSpeed * Time.deltaTime
    );

        FaceTarget(player.position);
    }


    private void AttackPlayer()
    {
        animator.SetBool("IsMoving", false);

        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            Invoke(nameof(ResetAttack), 0.8f); // match your attack animation length
        }

        FaceTarget(player.position);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

        private void FaceTarget(Vector3 target)
    
        {
            Vector3 direction = target - transform.position;
            if (Mathf.Abs(direction.x) < 0.2f)
            return;

            if (direction.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    
            else if (direction.x > 0)
            transform.localScale = originalScale;
        }

}

