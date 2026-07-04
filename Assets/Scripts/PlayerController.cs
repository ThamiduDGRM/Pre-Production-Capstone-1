using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;

    [SerializeField] private float moveSpeed = 5f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject attackHitbox;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;

    [SerializeField] private float jumpFreezeHeight = 0.05f;   // height at which we freeze again
    private float jumpStartY;

    private bool isGrounded;
    private float verticalVelocity;
    private bool jumpPressed;

    private PlayerControls playerControls;
    private Rigidbody rb;

    private Vector3 movement;
    private Vector2 moveInput;
    public Vector3 direction;

    private const string IS_MOVING_PARAM = "IsMoving";
    private const string ATTACK_TRIGGER = "Attack";

    private bool isAttacking = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Attack.performed += ctx => OnAttack();
        playerControls.Player.Drop.performed += ctx => OnDropBomb();
        playerControls.Player.ShootFireball.performed += ctx => OnShootFireball();

        // Store jump input instead of jumping immediately
        playerControls.Player.Jump.performed += ctx => jumpPressed = true;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackHitbox.SetActive(false);

        // Freeze Y at start so player stays on ground plane
        FreezeY();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
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

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log("Grounded: " + isGrounded);

        // Gravity
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else if (verticalVelocity < 0)
        {
            verticalVelocity = -2f; // keeps player grounded
        }

        // Jump logic (only once per press)
        if (jumpPressed && isGrounded)
        {
            UnfreezeY();                     // allow vertical movement
            animator.SetTrigger("Jump");

            jumpStartY = rb.position.y;      // record starting height
            verticalVelocity = jumpForce;    // launch upward
        }

        jumpPressed = false; // consume jump input

        // ⭐ HEIGHT-BASED FREEZE LOGIC ⭐
        // When player returns close to ground height, freeze Y again
        if (rb.position.y <= jumpStartY + jumpFreezeHeight)
        {
            FreezeY();
        }
        else
        {
            UnfreezeY(); // allow upward/downward movement
        }
    }

    private void FixedUpdate()
    {
        // Horizontal movement
        Vector3 horizontal = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;

        // Vertical movement
        Vector3 vertical = new Vector3(0, verticalVelocity * Time.fixedDeltaTime, 0);

        rb.MovePosition(rb.position + horizontal + vertical);
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
        }
    }

    private void OnShootFireball()
    {
        if (!PlayerStats.Instance.fireballActive) return;

        animator.SetTrigger("FireballCast");
        StartCoroutine(FireballRoutine());
    }

    private IEnumerator FireballRoutine()
    {
        yield return new WaitForSeconds(0.25f);

        Vector3 dir = playerSprite.flipX ? Vector3.left : Vector3.right;

        GameObject go = Instantiate(PlayerStats.Instance.fireballPrefab,
                                    PlayerStats.Instance.fireballSpawnPoint.position,
                                    Quaternion.identity);

        Fireball fb = go.GetComponent<Fireball>();
        fb.direction = dir;
    }

    // ⭐ FREEZE / UNFREEZE HELPERS ⭐
    private void FreezeY()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;
    }

    private void UnfreezeY()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.15f);
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);

        yield return new WaitForSeconds(0.45f);
        isAttacking = false;
    }
}










