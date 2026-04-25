using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;

    [SerializeField] private float moveSpeed = 5f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject attackHitbox;

    private PlayerControls playerControls;
    private Rigidbody rb;

    private Vector3 movement;
    private Vector2 moveInput;

    private const string IS_MOVING_PARAM = "IsMoving";
    private const string ATTACK_TRIGGER = "Attack";

    private bool isAttacking = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Attack.performed += ctx => OnAttack();
        playerControls.Player.Drop.performed += ctx => OnDropBomb();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackHitbox.SetActive(false); // ensure hitbox starts disabled
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        // Read movement input into the CLASS VARIABLE (not a new local one)
        moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        float x = moveInput.x;
        float z = moveInput.y;

        movement = new Vector3(x, 0, z).normalized;
        animator.SetBool(IS_MOVING_PARAM, movement != Vector3.zero);

        // Flip sprite + hitbox
        if (x < 0)
        {
            playerSprite.flipX = true;
            attackHitbox.transform.localPosition = new Vector3(
                -Mathf.Abs(attackHitbox.transform.localPosition.x),
                attackHitbox.transform.localPosition.y,
                attackHitbox.transform.localPosition.z
            );
        }
        else if (x > 0)
        {
            playerSprite.flipX = false;
            attackHitbox.transform.localPosition = new Vector3(
                Mathf.Abs(attackHitbox.transform.localPosition.x),
                attackHitbox.transform.localPosition.y,
                attackHitbox.transform.localPosition.z
            );
        }
    }

    private void FixedUpdate()
    {
        // Move using Rigidbody.MovePosition (collisions work even when kinematic)
        Vector3 targetPos = rb.position + new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    private void OnAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger(ATTACK_TRIGGER);
        StartCoroutine(AttackRoutine());
    }
    
        private void OnDropBomb()
    {
        if (PlayerStats.Instance != null)
      {
        bool dropped = PlayerStats.Instance.DropBomb();
        if (dropped)
        {
            
        }
     }
}
    private IEnumerator AttackRoutine()
    {
        // Enable hitbox at the correct frame
        yield return new WaitForSeconds(0.15f);
        attackHitbox.SetActive(true);

        // Keep hitbox active for the hit window
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);

        // Cooldown before next attack
        yield return new WaitForSeconds(0.45f);
        isAttacking = false;
    }
}





