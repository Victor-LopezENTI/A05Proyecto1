using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : IPlayerState
{
    // Constants
    private const float HorizontalSpeed = 9f;
    private const float HorizontalAcceleration = 13f;
    private const float HorizontalDeceleration = 16f;
    private const float HorizontalVelocityPower = 0.96f;
    private const float DistanceFromGround = 0.85f;

    // Properties
    private Rigidbody2D playerRb => PlayerStateMachine.instance.playerRb;
    private float _timeInAir;

    public void OnEnter()
    {
        PlayerStateMachine.instance.canMoveInAir = true;
    }

    public void Update()
    {
        if (playerRb.velocity.y > 0)
        {
            PlayerStateMachine.instance.playerAnimator.Play("jump");
        }
        else if (playerRb.velocity.y < 0)
        {
            PlayerStateMachine.instance.playerAnimator.Play("fall");
        }
    }

    public void FixedUpdate()
    {
        // Horizontal movement
        if (PlayerStateMachine.instance.canMoveInAir && PlayerInput.instance.horizontalInput != 0)
        {
            var targetSpeed = PlayerInput.instance.horizontalInput * HorizontalSpeed;
            var speedDifference = targetSpeed - playerRb.velocity.x;
            var accelerationRate = targetSpeed != 0 ? HorizontalAcceleration : HorizontalDeceleration;
            var movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, HorizontalVelocityPower) *
                           Mathf.Sign(speedDifference);
            playerRb.AddForce(movement * Vector2.right);
        }

        _timeInAir += Time.deltaTime;
        if (_timeInAir > 0.1f)
        {
            if (PlayerStateMachine.instance.OnGround && playerRb.velocity.y <= 1f)
            {
                if (PlayerInput.instance.horizontalInput == 0f)
                {
                    PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
                }
                else
                {
                    PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
                }
            }
        }

        if (PlayerInput.instance.jumpInput > 0f)
        {
            bool canJumpBeforeGround =
                Physics2D.Raycast(playerRb.position, Vector2.down, DistanceFromGround * 5f,
                    PlayerStateMachine.instance.groundLayer);
            if (canJumpBeforeGround)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
            }
        }
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        if (PlayerStateMachine.instance.OnGround)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (RopeManager.Instance.selectedHook)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.RopingState);
        }
    }

    public void OnExit()
    {
        _timeInAir = 0;
    }
}