using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private AudioSource dashAudio;
    [SerializeField] private LevelCountdown countdown;


    [Header("Attack Settings")]
    [SerializeField] private GameObject attackHitbox;

    // ---------------- DASH SETTINGS ----------------
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing = false;
    private bool canDash = true;
    private Vector3 dashDirection;

    // ---------------- AFTERIMAGE SETTINGS ----------------
    [Header("Afterimage Settings")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostSpawnInterval = 0.03f;
    [SerializeField] private float ghostLifetime = 0.25f;
    [SerializeField] private Color ghostColor = new Color(1f, 1f, 1f, 0.6f);
    // -----------------------------------------------------

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
        playerControls.Player.ShootFireball.performed += ctx => OnShootFireball();

        // DASH INPUT (Left Shift)
        playerControls.Player.Dash.performed += ctx => OnDash();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackHitbox.SetActive(false);
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
 {
    // BLOCK ALL GAMEPLAY UNTIL COUNTDOWN IS DONE
    if (!countdown.countdownFinished)
        return;

    moveInput = playerControls.Player.Move.ReadValue<Vector2>();

    float x = moveInput.x;
    float z = moveInput.y;

    movement = new Vector3(x, 0, z).normalized;
    animator.SetBool(IS_MOVING_PARAM, movement != Vector3.zero);

    // Flip sprite + shadow + hitbox
    if (x < 0)
    {
        playerSprite.flipX = true;
        shadowTransform.localScale = new Vector3(
            -Mathf.Abs(shadowTransform.localScale.x),
            shadowTransform.localScale.y,
            shadowTransform.localScale.z
        );

        attackHitbox.transform.localPosition = new Vector3(
            -Mathf.Abs(attackHitbox.transform.localPosition.x),
            attackHitbox.transform.localPosition.y,
            attackHitbox.transform.localPosition.z
        );
    }
    else if (x > 0)
    {
        playerSprite.flipX = false;
        shadowTransform.localScale = new Vector3(
            Mathf.Abs(shadowTransform.localScale.x),
            shadowTransform.localScale.y,
            shadowTransform.localScale.z
        );

        attackHitbox.transform.localPosition = new Vector3(
            Mathf.Abs(attackHitbox.transform.localPosition.x),
            attackHitbox.transform.localPosition.y,
            attackHitbox.transform.localPosition.z
        );
    }
 }


    private void FixedUpdate()
    {
        // ---------------- DASH MOVEMENT OVERRIDE ----------------
        if (isDashing)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            return;
        }
        // ---------------------------------------------------------

        // Normal movement
        Vector3 horizontal = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + horizontal);
    }

    // ---------------- DASH LOGIC ----------------
    private void OnDash()
    {
        if (!canDash || isDashing) return;

        dashAudio.Play();   // Play dash sound

        isDashing = true;
        canDash = false;

        // Dash direction: movement OR facing direction
        dashDirection = movement.sqrMagnitude > 0.1f
            ? movement.normalized
            : (playerSprite.flipX ? Vector3.left : Vector3.right);

        StartCoroutine(DashRoutine());
        StartCoroutine(SpawnGhostsDuringDash());
    }

    private IEnumerator DashRoutine()
    {
        float timer = dashDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        StartCoroutine(DashCooldownRoutine());
    }

    private IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    // ---------------------------------------------------------

    // ---------------- AFTERIMAGE LOGIC ----------------
    private IEnumerator SpawnGhostsDuringDash()
    {
        while (isDashing)
        {
            SpawnGhost();
            yield return new WaitForSeconds(ghostSpawnInterval);
        }
    }

    private void SpawnGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);

        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();
        ghostSR.sprite = playerSprite.sprite;
        ghostSR.flipX = playerSprite.flipX;
        ghostSR.color = ghostColor;

        StartCoroutine(FadeAndDestroyGhost(ghostSR));
    }

    private IEnumerator FadeAndDestroyGhost(SpriteRenderer ghostSR)
    {
        float timer = ghostLifetime;
        Color c = ghostSR.color;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            c.a = Mathf.Lerp(0f, ghostColor.a, timer / ghostLifetime);
            ghostSR.color = c;
            yield return null;
        }

        Destroy(ghostSR.gameObject);
    }
    // ---------------------------------------------------------

    private void OnAttack()
    {
        
        if (!countdown.countdownFinished)
        return;
        
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger(ATTACK_TRIGGER);
        StartCoroutine(AttackRoutine());
    }

    private void OnDropBomb()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.DropBomb();
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

        GameObject go = Instantiate(
            PlayerStats.Instance.fireballPrefab,
            PlayerStats.Instance.fireballSpawnPoint.position,
            Quaternion.identity
        );

        Fireball fb = go.GetComponent<Fireball>();
        fb.direction = dir;
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.15f);
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);

        yield return new WaitForSeconds(0.08f);
        isAttacking = false;
    }
    
    public void DisableInput()
    {
        playerControls.Disable();
    }

    public void EnableInput()
    {
        playerControls.Enable();
    }








}













