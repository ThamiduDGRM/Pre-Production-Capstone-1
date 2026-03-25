using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float chaseSpeed = 3.5f;
    public float detectionRange = 6f;
    public float attackRange = 1.5f;
    public Animator animator;

    private Transform player;
    private bool isAttacking = false;
    private Vector3 originalScale;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        player = p.transform;

        originalScale = transform.localScale;
    }


    private void FixedUpdate()
    {
        if (player == null) return;
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
            animator.SetBool("IsMoving", false); // idle
        }
    }

    private void ChasePlayer()
    {
        if (isAttacking) return;

        animator.SetBool("IsMoving", true);

        Vector3 targetPos = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            chaseSpeed * Time.deltaTime
        );

        FaceTarget(player.position);
    }

    
    private void DealDamageToPlayer()
    {
        IDamageable dmg = player.GetComponent<IDamageable>();
          if (dmg != null)
        {
            dmg.TakeDamage(1);
        }
    }

    
    
    
    
    private void AttackPlayer()
    {
        animator.SetBool("IsMoving", false);

        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            Invoke(nameof(DealDamageToPlayer), 0.3f);
            Invoke(nameof(ResetAttack), 0.8f);
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
        else
            transform.localScale = originalScale;
    }
}


