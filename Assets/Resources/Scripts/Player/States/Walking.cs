using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : IPlayerState
{
    // Movement constants
    private const float WalkSpeed = 9f;
    private const float Acceleration = 13f;
    private const float Deceleration = 16f;
    private const float VelocityPower = 0.96f;

    public void OnEnter()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled += OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
        InputManager.PlayerInputActions.Player.Click.performed += OnClickInputPerformed;
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

        PlayerStateMachine.instance.playerAnimator.Play("walk");
    }

    public void FixedUpdate()
    {
        // Movement
        var targetSpeed = PlayerStateMachine.instance.horizontalInput * WalkSpeed;
        var speedDifference = targetSpeed - PlayerStateMachine.instance.playerRb.velocity.x;
        var accelerationRate = targetSpeed != 0 ? Acceleration : Deceleration;
        var movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, VelocityPower) *
                       Mathf.Sign(speedDifference);

        PlayerStateMachine.instance.playerRb.AddForce(movement * Vector2.right);

        if (!PlayerStateMachine.instance.onGround)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
        }

        if (PlayerStateMachine.instance.horizontalInput == 0)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
        }
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.horizontalInput = context.ReadValue<float>();
    }

    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
    }

    private void OnClickInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.clickInput = context.ReadValue<float>();
    }

    public void OnExit()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled -= OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
        InputManager.PlayerInputActions.Player.Click.performed -= OnClickInputPerformed;
    }
}