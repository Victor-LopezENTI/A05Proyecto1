using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : IPlayerState
{
    // Movement constants
    private const float WalkSpeed = 9f;
    private const float Acceleration = 13f;
    private const float VelocityPower = 0.96f;

    private Rigidbody2D playerRb => PlayerStateMachine.instance.playerRb;

    public void OnEnter()
    {
        PlayerStateMachine.instance.groundCheckDistance = 0.45f;

        PlayerInput.instance.PlayerInputActions.Player.HorizontalMovement.canceled += OnMovementInputCanceled;
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
        var movement = Mathf.Pow(Mathf.Abs(speedDifference) * Acceleration, VelocityPower) *
                       Mathf.Sign(speedDifference) * Vector2.right;

        // Ramp vertical compensation force
        if (playerRb.velocity.y >= 0.8f)
        {
            movement.y += 85f;
        }

        PlayerStateMachine.instance.playerRb.AddForce(movement);

        if (!PlayerStateMachine.instance.OnGround)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
        }

        if (PlayerInput.instance.jumpInput > 0)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
        }
    }

    private void OnMovementInputCanceled(InputAction.CallbackContext context)
    {
        PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
    }

    public void OnExit()
    {
        PlayerStateMachine.instance.groundCheckDistance = 0.1f;

        PlayerInput.instance.PlayerInputActions.Player.HorizontalMovement.canceled -= OnMovementInputCanceled;
    }
}