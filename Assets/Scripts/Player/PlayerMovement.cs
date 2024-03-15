using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public static PlayerMovement instance;

    [SerializeField]
    public enum PlayerState
    {
        Idle,
        Charging,
        Jumping
    };

    [SerializeField] PlayerState nowState, lastState;

    // MANAGERS
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Collider2D playerCollider;

    // MOVEMENT VARIABLES

    // Jump logic
    [SerializeField] private bool onGround;
    private bool jumpInput;
    [SerializeField] private float holdTimer, maxHoldTime;

    // Movement
    [SerializeField] public float moveSpeed, jumpForce;

    // Interact
    private bool interactInput;

    // GROUNDCHECK
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] float distanceFromGround;
    Vector2 boxCastSize;

    #endregion

    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;

        // Get Components
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        distanceFromGround = 0.185f;
        boxCastSize = new(0.14f, 0.1f);

        jumpForce = 300f;
        maxHoldTime = 1f;   // Max holding time in seconds

        nowState = PlayerState.Idle;
    }

    private void Update()
    {
        Debug.DrawRay(groundCheck.transform.position, new Vector2(0f, -distanceFromGround), Color.green);
    }

    private void FixedUpdate()
    {
        onGround = isGrounded();    // Check if the player is touching the ground

        // Turn the inputs to booleans
        jumpInput = inputManager.jumpInput == 1;
        interactInput = inputManager.interactInput == 1;

        // Hold jump logic
        if (!jumpInput && lastState == PlayerState.Charging)
        {
            // Jumping state
            nowState = PlayerState.Jumping;
            moveSpeed = 400f;
            playerRB.velocity = new(inputManager.moveInput * moveSpeed * Time.deltaTime, jumpForce * holdTimer * Time.deltaTime);
        }

        if (jumpInput && onGround)
        {
            // Charging state
            nowState = PlayerState.Charging;
            moveSpeed = 150f;

            holdTimer += Time.deltaTime;
            if (holdTimer > maxHoldTime)
                holdTimer = maxHoldTime;
        }

        else if (onGround && lastState != PlayerState.Charging)
        {
            // Idle state
            nowState = PlayerState.Idle;
            holdTimer = 0f;
            moveSpeed = 400f;
        }

        playerRB.velocity = new(inputManager.moveInput * moveSpeed * Time.deltaTime, playerRB.velocity.y);

        lastState = nowState;
    }

    public bool isGrounded()
    {
        return Physics2D.BoxCast(groundCheck.transform.position, boxCastSize, 0f, Vector2.down, distanceFromGround, groundLayer);
    }
}