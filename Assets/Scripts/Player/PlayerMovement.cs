using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public static PlayerMovement instance;

    private PlayerController playerController;

    [SerializeField] private Rigidbody2D playerRB;

    [SerializeField] private Collider2D playerCollider;

    // MOVEMENT VARIABLES
    private float moveInput, jumpInput, jumpThreshold, interacted;
    [SerializeField] public float moveSpeed, jumpForce;
    [SerializeField] private bool onGround, jumping;
    private Vector2 playerVelocity;

    // Groundcheck
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] float distanceFromGround;
    Vector2 boxCastSize;

    #endregion

    private void Awake()
    {
        if (instance == null) instance = this;

        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        playerController = new PlayerController();
        playerController.Enable();
    }

    private void Start()
    {
        distanceFromGround = 0.185f;
        boxCastSize = new Vector2(0.14f, 0.1f);

        moveSpeed = 500f;
        jumpForce = 300f;
    }

    private void Update()
    {
        // Track the inputs
        moveInput = playerController.Player.MovimientoHorizontal.ReadValue<float>();
        jumpInput = playerController.Player.Salto.ReadValue<float>();

        interacted = playerController.Player.Interactuar.ReadValue<float>();

        Debug.DrawRay(groundCheck.transform.position, new Vector2(0f, -distanceFromGround), Color.green);
    }

    private void FixedUpdate()
    {
        onGround = isGrounded();    // Check if the player is touching the ground

        jumping = jumpInput == 1;

        // Jump
        if (jumping)
        {
            playerVelocity = new Vector2(moveInput * moveSpeed * Time.deltaTime, jumpInput * jumpForce * Time.deltaTime);
        }
        else
        {
            playerVelocity = new Vector2(moveInput * moveSpeed * Time.deltaTime, playerRB.velocity.y);
        }

        playerRB.velocity = playerVelocity;

        // Jump interaction test
        if (interacted == 1)
        {
            transform.DOJump(new Vector2(playerRB.position.x + 10f, playerRB.position.y), 2f, 1, 0.8f).SetEase(Ease.Linear);
        }
    }

    public bool isGrounded()
    {
        return Physics2D.BoxCast(groundCheck.transform.position, boxCastSize, 0f, Vector2.down, distanceFromGround, groundLayer);
    }

    // Horizontal movement input
    public void onMove(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
    }

    // Jump input
    public void onJump(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
    }

    public void onInteract(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
    }
}