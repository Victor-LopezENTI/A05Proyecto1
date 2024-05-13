using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : IPlayerState
{
    // Horizontal movement constants
    private const float HorizontalSpeed = 9f;
    private const float HorizontalAcceleration = 13f;
    private const float HorizontalDeceleration = 16f;
    private const float HorizontalVelocityPower = 0.96f;

    private const float DistanceFromGround = 0.85f;

    private readonly Rigidbody2D _playerRb = PlayerStateMachine.instance.playerRb;
    private float _timeInAir;

    public void OnEnter()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled += OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
        InputManager.PlayerInputActions.Player.Click.performed += OnClickPerformed;
    }

    public void Update()
    {
        switch (PlayerStateMachine.instance.horizontalInput)
        {
            case > 0:
                PlayerStateMachine.instance.transform.localScale = new Vector3(1, 1, 1);
                break;
            case < 0:
                PlayerStateMachine.instance.transform.localScale = new Vector3(-1, 1, 1);
                break;
        }

        if (_playerRb.velocity.y > 0)
        {
            PlayerStateMachine.instance.playerAnimator.Play("jump");
        }
        else if (_playerRb.velocity.y < 0)
        {
            PlayerStateMachine.instance.playerAnimator.Play("fall");
        }
    }

    public void FixedUpdate()
    {
        // Horizontal movement
        if (PlayerStateMachine.instance.canMoveInAir)
        {
            var targetSpeed = PlayerStateMachine.instance.horizontalInput * HorizontalSpeed;
            var speedDifference = targetSpeed - PlayerStateMachine.instance.playerRb.velocity.x;
            var accelerationRate = targetSpeed != 0 ? HorizontalAcceleration : HorizontalDeceleration;
            var movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, HorizontalVelocityPower) *
                           Mathf.Sign(speedDifference);
            _playerRb.AddForce(movement * Vector2.right);
        }

        _timeInAir += Time.deltaTime;
        if (PlayerStateMachine.instance.onGround && _timeInAir > 0.1f)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
        }

        if (PlayerStateMachine.instance.jumpInput > 0f)
        {
            bool canJumpBeforeGround =
                Physics2D.Raycast(_playerRb.position, Vector2.down, DistanceFromGround * 5f,
                    PlayerStateMachine.instance.groundLayer);
            if (canJumpBeforeGround)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
            }
        }
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.horizontalInput = context.ReadValue<float>();

        if (PlayerStateMachine.instance.onGround)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
        }
    }

    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.jumpInput = context.ReadValue<float>();
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.clickInput = context.ReadValue<float>();
        if (PlayerStateMachine.instance.selectedHook != null)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.RopingState);
        }
    }

    public void OnExit()
    {
        _timeInAir = 0;

        PlayerStateMachine.instance.canMoveInAir = true;

        InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled -= OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
        InputManager.PlayerInputActions.Player.Click.performed -= OnClickPerformed;
    }
}