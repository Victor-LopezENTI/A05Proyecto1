using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance { get; private set; }

    public enum PlayerState
    {
        Idle,
        Walking,
        ChargingJump,
        StartingJump,
        Jumping,
        Falling,

        // Slingshot states
        ChargingSlingshot,
        StartingSlingshot,

        //Rope States
        Roping

    };

    // Player states variables
    [SerializeField] private PlayerState m_currentState;
    public PlayerState currentState { get => m_currentState; private set => m_currentState = value; }
    private PlayerState lastState;

    // Player input variables
    private float moveInput;
    private bool jumpInput;

    // Groundcheck variables
    [SerializeField] private LayerMask groundLayer;
    private const float distanceFromGround = 0.75f;
    public bool onGround { get; private set; }

    // Slingshot variables
    private SlingshotJump slingshotJump;

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

        slingshotJump = GetComponent<SlingshotJump>();
    }

    private void FixedUpdate()
    {
        // Groundcheck
        onGround = Physics2D.Raycast(transform.position, Vector2.down, distanceFromGround, groundLayer);

        // Get the inputs from InputManager
        moveInput = InputManager.Instance.moveInput;
        jumpInput = InputManager.Instance.jumpInput;

        if (onGround)
        {
            if (jumpInput)
            {
                // ChargingJump
                if (!slingshotJump.onSlingShot)
                    currentState = PlayerState.ChargingJump;

                // ChargingSlingshot
                else if (slingshotJump.onSlingShot)
                    currentState = PlayerState.ChargingSlingshot;
            }
            else
            {
                // StartingJump
                if (lastState == PlayerState.ChargingJump)
                    currentState = PlayerState.StartingJump;

                // StartingSlingshot
                else if (lastState == PlayerState.ChargingSlingshot)
                    currentState = PlayerState.StartingSlingshot;

                // Idle
                else if (moveInput == 0)
                    currentState = PlayerState.Idle;

                // Walking
                else if (moveInput != 0)
                    currentState = PlayerState.Walking;
            }
        }
        else
        {
            if (GetComponent<RopeManager>().hingeConnected)
            {
                currentState = PlayerState.Roping;
            }
            // Ascending
            else if (PlayerMovement.Instance.playerRB.velocity.y >= 0)
                currentState = PlayerState.Jumping;

            // Falling
            else if (PlayerMovement.Instance.playerRB.velocity.y < 0)
                currentState = PlayerState.Falling;
        }

        lastState = currentState;
    }
}