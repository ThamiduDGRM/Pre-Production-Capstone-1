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

    public int damageToPlayer = 1;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        // --- BLOCK AI UNTIL COUNTDOWN FINISHES ---
        if (!LevelCountdown.Instance.countdownFinished)
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        // --- KEEP ENEMIES FROM PILING TOGETHER ---
        MaintainSpacing();

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

    // ---------------------------------------------------------
    // SPACING LOGIC — prevents enemies from stacking together
    // ---------------------------------------------------------
    private void MaintainSpacing()
    {
        float minDistance = 4f;      // how far apart enemies should stay
        float pushStrength = 1f;       // how strongly they separate

        Collider[] nearby = Physics.OverlapSphere(transform.position, minDistance);

        foreach (Collider col in nearby)
        {
            if (col.gameObject == this.gameObject) continue;
            if (!col.CompareTag("Enemy")) continue;

            Vector3 away = transform.position - col.transform.position;
            away.y = 0; // keep movement flat

            transform.position += away.normalized * pushStrength * Time.deltaTime;
        }
    }

    private void ChasePlayer()
    {
        if (isAttacking) return;

        animator.SetBool("IsMoving", true);

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
            chaseSpeed * Time.deltaTime
        );

        FaceTarget(player.position);
    }

    private void DealDamageToPlayer()
    {
        IDamageable dmg = player.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damageToPlayer);
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





