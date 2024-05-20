using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class ChargingJump : IPlayerState
{
    // Constants
    private const float HoldTimeMin = 0.33f;
    private const float HoldTimeMax = 0.5f;
    private const float JumpForceMin = 1200f;
    private const float JumpForceMax = 1800f;

    // Properties
    private Rigidbody2D playerRb => PlayerStateMachine.instance.playerRb;
    private float _holdTimer;
    private float _holdTimerNormalized;
    private Vector2 _jumpForceVector;

    public void OnEnter()
    {
        PlayerStateMachine.instance.chargeBar.fillAmount = 0f;
        PlayerStateMachine.instance.playerUi.transform.localScale = playerRb.transform.lossyScale.x < 0
            ? new Vector3(-0.1f, 0.1f, 0.1f)
            : new Vector3(0.1f, 0.1f, 0.1f);

        PlayerStateMachine.instance.playerUi.enabled = true;

        PlayerInput.instance.PlayerInputActions.Player.Jump.canceled += OnJumpInputCanceled;
    }

    public void Update()
    {
        PlayerStateMachine.instance.playerAnimator.Play("charge_jump");
    }

    public void FixedUpdate()
    {
        playerRb.velocity = new Vector2(0f, playerRb.velocity.y);

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

    private void OnJumpInputCanceled(InputAction.CallbackContext context)
    {
        if (_holdTimer < HoldTimeMin)
        {
            _jumpForceVector = JumpForceMin * Vector2.up;
        }

        AudioManager.Instance.PlaySFX("Jump");
        if (playerRb.velocity.y >= 0f)
        {
            playerRb.AddForce(_jumpForceVector);
        }

        _jumpForceVector = Vector2.zero;
        _holdTimer = 0;

        PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
    }

    public void OnExit()
    {
        PlayerStateMachine.instance.playerUi.enabled = false;

        PlayerInput.instance.PlayerInputActions.Player.Jump.canceled -= OnJumpInputCanceled;
    }
}