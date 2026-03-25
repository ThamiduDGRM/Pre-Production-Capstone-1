using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Attack Settings")]
    [SerializeField] private GameObject attackHitbox; // <-- ADD THIS

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;

    private const string IS_MOVING_PARAM = "IsMoving";
    private const string ATTACK_TRIGGER = "Attack";

    private bool isAttacking = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Attack.performed += ctx => OnAttack();
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
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        float x = moveInput.x;
        float z = moveInput.y;

        movement = new Vector3(x, 0, z).normalized;
        animator.SetBool(IS_MOVING_PARAM, movement != Vector3.zero);

        // Flip sprite based on direction
        if (x < 0)
            playerSprite.flipX = true;
        else if (x > 0)
            playerSprite.flipX = false;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger(ATTACK_TRIGGER);

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        // Enable hitbox at the correct frame
        yield return new WaitForSeconds(0.15f); // adjust to match animation
        attackHitbox.SetActive(true);

        // Keep hitbox active for the hit window
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);

        // Cooldown before next attack
        yield return new WaitForSeconds(0.45f);
        isAttacking = false;
    }
}




