using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    #region Variables

    PlayerStateMachine playerStateMachine;

    // Player components
    public Rigidbody2D playerRB { get; private set; }
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;
    private SlingshotJump slingshotJump;

    // Input variables
    private float moveInput;

    // Jump timer variables
    [SerializeField]
    private float holdTimer;
    private const float maxHoldTime = 1f;

    // Movement variables
    public bool facingRight { get; private set; }
    private float moveSpeed;
    private const float moveSpeedWalk = 400f;
    private const float moveSpeedChargeJump = 150f;
    private const float moveSpeedJump = 330f;

    // Jump variables
    private const float jumpForce = 400;
    private const float minJumpForce = 200f;

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
        }

        #endregion

        // Get components
        playerStateMachine = PlayerStateMachine.Instance;
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        slingshotJump = GetComponent<SlingshotJump>();
    }

    private void Update()
    {
        // Flip the player sprite
        if (moveInput != 0)
        {
            facingRight = moveInput < 0;
            playerSprite.flipX = facingRight;
        }
    }

    private void FixedUpdate()
    {
        // Get the inputs from InputManager
        moveInput = InputManager.Instance.moveInput;

        // Switch all possibla PlayerStates
        switch (playerStateMachine.currentState)
        {
            case PlayerStateMachine.PlayerState.Idle:
                holdTimer = 0f;
                playerAnimator.Play("idle");
                break;

            case PlayerStateMachine.PlayerState.Walking:
                holdTimer = 0f;
                moveSpeed = moveSpeedWalk;
                playerAnimator.Play("walk");
                break;

            case PlayerStateMachine.PlayerState.ChargingJump:
                moveSpeed = moveSpeedChargeJump;
                holdTimer += Time.deltaTime;
                if (holdTimer > maxHoldTime)
                    holdTimer = maxHoldTime;
                playerAnimator.Play("idle");
                break;

            case PlayerStateMachine.PlayerState.StartingJump:
                if (holdTimer < 0.25f)
                    playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, minJumpForce * Time.deltaTime);
                else
                    playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, jumpForce * holdTimer * Time.deltaTime);
                break;

            case PlayerStateMachine.PlayerState.Jumping:
                holdTimer = 0f;
                moveSpeed = moveSpeedJump;
                playerAnimator.Play("jump");
                break;

            case PlayerStateMachine.PlayerState.Falling:
                playerAnimator.Play("fall");
                break;

            case PlayerStateMachine.PlayerState.ChargingSlingshot:
                moveSpeed = 0f;
                break;

            case PlayerStateMachine.PlayerState.StartingSlingshot:
                moveSpeed = 0f;
                playerRB.velocity = slingshotJump.velocity;
                break;
        }

        playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, playerRB.velocity.y);
    }
}