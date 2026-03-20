using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private Animator animator;

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 lastDirection = Vector3.forward;

    private void Awake()
    {
        playerControls = new PlayerControls();
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
        // FIXED: Use .Move instead of .Movement
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        float x = moveInput.x;
        float z = moveInput.y;

        movement = new Vector3(x, 0, z).normalized;

        // Animator parameters
        animator.SetFloat("MoveX", x);
        animator.SetFloat("MoveZ", z);

        if (movement.magnitude > 0.1f)
        {
            lastDirection = movement;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("IdleX", lastDirection.x);
            animator.SetFloat("IdleZ", lastDirection.z);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }
}

