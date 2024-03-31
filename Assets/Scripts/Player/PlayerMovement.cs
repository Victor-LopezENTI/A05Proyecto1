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

    private PlayerStateMachine playerStateMachine;
    private Rigidbody2D playerRB;

    // Input
    private bool jumpInput;
    private bool interactInput;
    private float moveInput;

    // Jump timer
    [SerializeField] private float holdTimer;
    private const float maxHoldTime = 1f;

    // Movement speed
    private float moveSpeed = 400f;
    private float jumpForce = 300f;
    private float minJumpForce = 2.3f;

    #endregion

    private void Awake()
    {
        // Get components
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get the inputs from InputManager
        jumpInput = InputManager.Instance.getJumpInput() == 1;
        interactInput = InputManager.Instance.getInteractInput() == 1;
        moveInput = InputManager.Instance.getMoveInput();

        switch (playerStateMachine.GetPlayerState())
        {
            case PlayerStateMachine.PlayerState.Idle:
                holdTimer = 0f;
                break;

            case PlayerStateMachine.PlayerState.Walking:
                holdTimer = 0f;
                moveSpeed = 400f;
                break;

            case PlayerStateMachine.PlayerState.ChargingJump:
                moveSpeed = 150f;
                holdTimer += Time.deltaTime;
                if (holdTimer > maxHoldTime)
                    holdTimer = maxHoldTime;
                break;

            case PlayerStateMachine.PlayerState.StartingJump:

                // Set the min jump force
                if (holdTimer > 0.25f)
                    minJumpForce = 0f;
                else
                    minJumpForce = 2.3f;

                playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, (jumpForce * holdTimer * Time.deltaTime) + minJumpForce);
                break;

            case PlayerStateMachine.PlayerState.Jumping:
                holdTimer = 0f;
                moveSpeed = 330f;
                break;

            case PlayerStateMachine.PlayerState.Falling:
                break;
        }

        playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, playerRB.velocity.y);
    }
}