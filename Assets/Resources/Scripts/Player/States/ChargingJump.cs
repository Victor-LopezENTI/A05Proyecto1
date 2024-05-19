using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class ChargingJump : IPlayerState
{
    private const float HoldTimeMin = 0.33f;
    private const float HoldTimeMax = 0.5f;
    private const float JumpForceMin = 1200f;
    private const float JumpForceMax = 1800f;

    private Rigidbody2D _playerRb = PlayerStateMachine.instance.playerRb;
    private float _holdTimer;
    private float _holdTimerNormalized;
    private Vector2 _jumpForceVector;
    
    public ChargingJump()
    {
        OnEnter();
    }

    public void OnEnter()
    {
        PlayerStateMachine.instance.chargeBar.enabled = true;

        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled += OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.canceled += OnJumpInputCanceled;
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

        PlayerStateMachine.instance.playerAnimator.Play("charge_jump");
    }

    public void FixedUpdate()
    {
        _playerRb.velocity = new Vector2(0f, _playerRb.velocity.y);

        _holdTimer += Time.deltaTime;
        if (_holdTimer > HoldTimeMax)
        {
            _holdTimer = HoldTimeMax;
        }

        _holdTimerNormalized = _holdTimer / HoldTimeMax;
        PlayerStateMachine.instance.chargeBar.fillAmount = _holdTimerNormalized;
        
        var movement = _holdTimerNormalized * JumpForceMax * Vector2.up;
        _jumpForceVector = movement;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerStateMachine.instance.horizontalInput = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            PlayerStateMachine.instance.horizontalInput = 0f;
            PlayerStateMachine.instance.horizontalInput = 0f;
        }
    }

    private void OnJumpInputCanceled(InputAction.CallbackContext context)
    {
        if (_holdTimer < HoldTimeMin)
        {
            _jumpForceVector = JumpForceMin * Vector2.up;
        }

        AudioManager.Instance.PlaySFX("Jump");
        _playerRb.AddForce(_jumpForceVector);
        _jumpForceVector = Vector2.zero;
        _holdTimer = 0;

        PlayerStateMachine.instance.jumpInput = context.ReadValue<float>();

        PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
    }

    public void OnExit()
    {
        PlayerStateMachine.instance.chargeBar.enabled = false;

        InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled -= OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.canceled -= OnJumpInputCanceled;
    }
}