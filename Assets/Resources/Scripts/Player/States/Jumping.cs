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
    private readonly LayerMask _groundLayer = LayerMask.GetMask("Platforms");

    private readonly Rigidbody2D _playerRb = PlayerStateMachine.instance.playerRb;
    private float _horizontalInput;
    private float _timeInAir;
    private bool _onGround;

    public Jumping()
    {
        OnEnter();
    }

    public void OnEnter()
    {
        _horizontalInput = PlayerStateMachine.instance.horizontalInput;

        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled += OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
    }

    public void Update()
    {
        switch (_horizontalInput)
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
        // Movement
        var targetSpeed = _horizontalInput * HorizontalSpeed;
        var speedDifference = targetSpeed - PlayerStateMachine.instance.playerRb.velocity.x;
        var accelerationRate = targetSpeed != 0 ? HorizontalAcceleration : HorizontalDeceleration;
        var movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, HorizontalVelocityPower) *
                       Mathf.Sign(speedDifference);
        _playerRb.AddForce(movement * Vector2.right);

        // Ground check
        _onGround = Physics2D.Raycast(_playerRb.position, Vector2.down,
            DistanceFromGround, _groundLayer);

        _timeInAir += Time.deltaTime;
        if (_onGround && _timeInAir > 0.1f)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
        }

        if (PlayerStateMachine.instance.jumpInput > 0f)
        {
            bool canJumpBeforeGround =
                Physics2D.Raycast(_playerRb.position, Vector2.down, DistanceFromGround * 4.5f, _groundLayer);
            if (canJumpBeforeGround)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
            }
        }
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<float>();

        if (_onGround)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
        }
    }

    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.jumpInput = context.ReadValue<float>();
    }

    public void OnExit()
    {
        PlayerStateMachine.instance.horizontalInput = _horizontalInput;
        _timeInAir = 0;

        InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled -= OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
    }
}