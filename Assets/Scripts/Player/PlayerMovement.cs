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

    public static PlayerMovement Instance;

    [SerializeField]
    private enum PlayerState
    {
        Idle,
        Charging,
        Jumping
    };

    private PlayerState nowState, lastState;

    [SerializeField] private Rigidbody2D playerRB;

    #region Movement Variables

    // Input
    private bool jumpInput;
    private bool interactInput;
    private float moveInput;

    // Jump logic
    [SerializeField] private bool onGround;
    [SerializeField] private float holdTimer, maxHoldTime;

    // Movement speed
    public float moveSpeed, jumpForce;

    #endregion

    // GROUNDCHECK
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] float distanceFromGround;
    Vector2 boxCastSize;

    #endregion

    private void Awake()
    {
        #region Singleton Pattern

        if (Instance != null)
        {
            Debug.Log("There is already an instance of " + Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        // Get components
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        distanceFromGround = 0.185f;
        boxCastSize = new(0.15f, 0.1f);

        jumpForce = 300f;
        maxHoldTime = 1f;   // Max holding time in seconds

        nowState = PlayerState.Idle;
    }

    private void FixedUpdate()
    {
        // Check if the player is touching the ground
        onGround = IsGrounded();

        // Get the inputs from InputManager
        jumpInput = InputManager.Instance.getJumpInput() == 1;
        interactInput = InputManager.Instance.getInteractInput() == 1;
        moveInput = InputManager.Instance.getMoveInput();

        #region State Logic

        if (!jumpInput && lastState == PlayerState.Charging)
        {
            // Jumping state
            nowState = PlayerState.Jumping;
            moveSpeed = 400f;
            playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, jumpForce * holdTimer * Time.deltaTime);
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

        // Change rigidbody velocity
        playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, playerRB.velocity.y);

        lastState = nowState;

        #endregion
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(groundCheck.transform.position, boxCastSize, 0f, Vector2.down, distanceFromGround, groundLayer);
    }
}