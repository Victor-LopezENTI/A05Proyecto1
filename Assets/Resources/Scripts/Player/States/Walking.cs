using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : IPlayerState
{
    // Movement constants
    private const float WalkSpeed = 9f;
    private const float Acceleration = 13f;
    private const float Deceleration = 16f;
    private const float VelocityPower = 0.96f;

    // Ground check constants
    private const float DistanceFromGround = 1f;

    private readonly Rigidbody2D _playerRb = PlayerStateMachine.instance.playerRb;
    private float _horizontalInput;

    private bool _onGround;
    private readonly LayerMask _groundLayer = LayerMask.GetMask("Platforms");

    public Walking()
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

        PlayerStateMachine.instance.playerAnimator.Play("walk");
    }

    public void FixedUpdate()
    {
        // Movement
        var targetSpeed = _horizontalInput * WalkSpeed;
        var speedDifference = targetSpeed - PlayerStateMachine.instance.playerRb.velocity.x;
        var accelerationRate = targetSpeed != 0 ? Acceleration : Deceleration;
        var movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, VelocityPower) *
                       Mathf.Sign(speedDifference);
        
        _playerRb.AddForce(movement * Vector2.right);

        // Ground check
        _onGround = Physics2D.Raycast(_playerRb.position, Vector2.down, DistanceFromGround, _groundLayer);

        if (!_onGround)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
        }
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<float>();

        if (context.canceled)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
        }
    }

    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
    }

    public void OnExit()
    {
        PlayerStateMachine.instance.horizontalInput = _horizontalInput;
        
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled -= OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
    }
}