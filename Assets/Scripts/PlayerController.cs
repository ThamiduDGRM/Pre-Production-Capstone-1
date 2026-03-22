using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;

    private const string IS_MOVING_PARAM = "IsMoving";
    private const string ATTACK_TRIGGER = "Attack";

    private void Awake()
    {
        playerControls = new PlayerControls();

        // Register attack input
        playerControls.Player.Attack.performed += ctx => OnAttack();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    private bool isAttacking = false;

    private void OnAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
    yield return new WaitForSeconds(0.4f); // matches animation and doesn't repeat attack animation for a 2nd time
    isAttacking = false;
    }

}



