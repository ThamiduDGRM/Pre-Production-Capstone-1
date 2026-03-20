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
    private Vector3 lastDirection = Vector3.forward;
    private const string IS_MOVING_PARAM = "IsMoving";
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
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        float x = moveInput.x;
        float z = moveInput.y;

        movement = new Vector3(x, 0, z).normalized;
        animator.SetBool(IS_MOVING_PARAM, movement!=Vector3.zero);

        if (x!=0 && x< 0)

        {
            playerSprite.flipX = true;
        }

        if (x!=0 && x> 0)

        {
            playerSprite.flipX = false;
        }


    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }
}


