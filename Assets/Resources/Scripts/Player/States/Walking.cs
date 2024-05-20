using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : IPlayerState
{
    // Movement constants
    private const float WalkSpeed = 9f;
    private const float Acceleration = 13f;
    private const float Deceleration = 16f;
    private const float VelocityPower = 0.96f;
    
    private Rigidbody2D playerRb => PlayerStateMachine.instance.playerRb;

    public void OnEnter()
    {
        PlayerStateMachine.instance.groundCheckDistance = 0.45f;

        PlayerInput.instance.PlayerInputActions.Player.HorizontalMovement.canceled += OnMovementInputCanceled;
        PlayerInput.instance.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
    }

    public void Update()
    {
        PlayerStateMachine.instance.playerAnimator.Play("walk");
    }

    public void FixedUpdate()
    {
        // Movement
        var targetSpeed = PlayerInput.instance.horizontalInput * WalkSpeed;
        var speedDifference = targetSpeed - playerRb.velocity.x;
        var accelerationRate = targetSpeed != 0 ? Acceleration : Deceleration;
        var movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, VelocityPower) *
                       Mathf.Sign(speedDifference) * Vector2.right;

        if (playerRb.velocity.y >= 0.8f)
        {
            movement.y += 97f;
        }

        PlayerStateMachine.instance.playerRb.AddForce(movement);

        if (!PlayerStateMachine.instance.OnGround)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
        }
    }

    private void OnMovementInputCanceled(InputAction.CallbackContext context)
    {
        PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
    }

    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
    }

    public void OnExit()
    {
        PlayerStateMachine.instance.groundCheckDistance = 0.1f;

        PlayerInput.instance.PlayerInputActions.Player.HorizontalMovement.canceled -= OnMovementInputCanceled;
        PlayerInput.instance.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
    }
}