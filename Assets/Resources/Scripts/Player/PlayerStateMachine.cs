using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine instance { get; private set; }

    public enum PlayerState
    {
        Idle,
        Walking,
        ChargingJump,
        StartingJump,
        Jumping,
        Falling,
        Paused,

        // Slingshot states
        ChargingSlingshot,
        StartingSlingshot,
        JumpingSlingshot,
        FallingSlingshot,

        // Rope States
        Roping,
        EnteringRope,
        LeavingRope
    };

    #region Variables

    // Player states
    [SerializeField] private PlayerState mCurrentState;
    public PlayerState currentState { get => mCurrentState; private set => mCurrentState = value; }
    
    private bool _clickInput;

    public bool isPaused = false;
    // Ground check variables
    [SerializeField] private LayerMask groundLayer;
    private const float DistanceFromGround = 1f;
    [SerializeField] private bool mOnGround;
    public bool onGround { get => mOnGround; private set => mOnGround = value; }
    
    #endregion

    private void Awake()
    {
        #region Singleton Pattern

        if (instance != null)
        {
            Debug.Log("There is already an instance of " + instance);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        #endregion
    }

    private void Start()
    {
        currentState = PlayerState.Idle;
    }

    private void OnEnable()
    {
        InputManager.MovementPerformed += OnMovementInput;
        InputManager.JumpPerformed += OnJumpInput;
    }

    private void OnMovementInput(Vector2 movement)
    {
        if (onGround)
        {
            currentState = PlayerState.Walking;
        }
    }
    
    private void OnJumpInput()
    {
        if (onGround)
        {
            currentState = PlayerState.ChargingJump;
        }
    }

    private void OnDisable()
    {
        InputManager.MovementPerformed -= OnMovementInput;
        InputManager.JumpPerformed -= OnJumpInput;
    }

    /*
    private void FixedUpdate()
    {
        // Ground check
        onGround = Physics2D.Raycast(transform.position, Vector2.down * RotationManager.instance.globalDirection, distanceFromGround, groundLayer);

        // Get the inputs from InputManager
        moveInput = InputManager.instance.moveInput;
        jumpInput = InputManager.instance.jumpInput;
        clickInput = InputManager.instance.clickInput;
        if (isPaused)
        {
            currentState = PlayerState.Paused;
        }
        else if (onGround)
        {
            // JumpingSlingshot
            if (lastState == PlayerState.StartingSlingshot)
                currentState = PlayerState.JumpingSlingshot;

            // ChargingJump
            else if (jumpInput && !clickInput)
                currentState = PlayerState.ChargingJump;

            // ChargingSlingshot
            else if (slingshotJump.chargingSlingshot)
                currentState = PlayerState.ChargingSlingshot;

            // StartingJump
            else if (lastState == PlayerState.ChargingJump)
                currentState = PlayerState.StartingJump;

            // StartingSlingshot
            else if (slingshotJump.startSlingshot)
                currentState = PlayerState.StartingSlingshot;

            // Walking
            else if (moveInput != 0)
                currentState = PlayerState.Walking;

            // Idle
            else if (moveInput == 0 && lastState != PlayerState.Jumping)
                currentState = PlayerState.Idle;
        }
        else
        {
            // EnteringRope
            if (ropeManager.enteringRope)
                currentState = PlayerState.EnteringRope;

            // LeavingRope
            else if (ropeManager.leavingRope)
                currentState = PlayerState.LeavingRope;

            // Roping
            else if (currentState != PlayerState.EnteringRope && currentState != PlayerState.LeavingRope && ropeManager.hingeConnected)
                currentState = PlayerState.Roping;

            else if (PlayerMovement.Instance.playerRB.velocity.y * RotationManager.instance.globalDirection.y >= 0)
            {
                // JumpingSlingshot
                if (slingshotJump.jumpingSlingshot)
                    currentState = PlayerState.JumpingSlingshot;

                // Jumping
                else
                    currentState = PlayerState.Jumping;
            }
            else if (PlayerMovement.Instance.playerRB.velocity.y * RotationManager.instance.globalDirection.y < 0)
            {
                // FallingSlingshot
                if (slingshotJump.jumpingSlingshot)
                    currentState = PlayerState.FallingSlingshot;

                // Falling
                else
                    currentState = PlayerState.Falling;
            }
        }
        lastState = currentState;
    }
    */
}