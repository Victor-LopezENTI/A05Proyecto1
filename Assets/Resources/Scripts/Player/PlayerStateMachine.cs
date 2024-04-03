using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Walking,
        ChargingJump,
        StartingJump,
        Jumping,
        Falling
    };

    // Player states
    [SerializeField] private PlayerState currentState = PlayerState.Idle;
    private PlayerState lastState;

    // Player input variables
    private float moveInput;
    private bool jumpInput;

    // Groundcheck variables
    [SerializeField] private bool onGround;
    [SerializeField] private LayerMask groundLayer;
    private const float distanceFromGround = 0.75f;

    private void FixedUpdate()
    {
        onGround = IsGrounded();

        moveInput = InputManager.Instance.moveInput;
        jumpInput = InputManager.Instance.jumpInput == 1;
        
        if (onGround)
        {
            // Jumping
            if (!jumpInput && lastState == PlayerState.ChargingJump)
                currentState = PlayerState.StartingJump;

            // ChargingJump
            else if (jumpInput)
                currentState = PlayerState.ChargingJump;

            // Idle
            else if (moveInput == 0)
                currentState = PlayerState.Idle;

            // Walking
            else if (moveInput != 0)
                currentState = PlayerState.Walking;
        }
        else
        {
            // Ascending
            if (PlayerMovement.Instance.playerRB.velocity.y >= 0)
                currentState = PlayerState.Jumping;

            // Falling
            else
                currentState = PlayerState.Falling;
        }

        lastState = currentState;
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, distanceFromGround, groundLayer);
    }

    public PlayerState GetPlayerState() { return currentState; }
}