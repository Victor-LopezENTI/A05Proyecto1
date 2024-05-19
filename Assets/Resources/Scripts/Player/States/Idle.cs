using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Screen = UnityEngine.Device.Screen;
using Vector2 = UnityEngine.Vector2;

public class Idle : IPlayerState
{
    private const float FrictionAmount = 2.3f;
    private Rigidbody2D playerRb => PlayerStateMachine.instance.playerRb;

    public void OnEnter()
    {
        playerRb.gravityScale = 0f;
        
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInputPerformed;
        InputManager.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
        InputManager.PlayerInputActions.Player.Click.performed += OnClickInput;
        InputManager.PlayerInputActions.Player.Click.canceled += OnClickInput;
    }

    public void Update()
    {
        PlayerStateMachine.instance.playerAnimator.Play("idle");
    }

    public void FixedUpdate()
    {
        playerRb.velocity = Vector2.zero;

        // var friction = Mathf.Min(Mathf.Abs(playerRb.velocity.x),
        //     Mathf.Abs(FrictionAmount));
        // friction *= Mathf.Sign(playerRb.velocity.x);
        // playerRb.AddForce(Vector2.right * -friction, ForceMode2D.Impulse);

        if (PlayerStateMachine.instance.horizontalInput != 0)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
        }
        else if (PlayerStateMachine.instance.jumpInput > 0)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
        }
    }

    private void OnMovementInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.horizontalInput = context.ReadValue<float>();
        PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
    }

    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.jumpInput = context.ReadValue<float>();
        PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
    }

    private void OnClickInput(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.clickInput = context.ReadValue<float>();

        if (context.performed && PlayerStateMachine.instance.onSlingshot)
        {
            var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Mouse.current.WarpCursorPosition(screenCenter);
            PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingSlingshotState);
        }
    }

    public void OnExit()
    {
        playerRb.gravityScale = 9.81f;
        
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnMovementInputPerformed;
        InputManager.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
        InputManager.PlayerInputActions.Player.Click.performed -= OnClickInput;
        InputManager.PlayerInputActions.Player.Click.canceled -= OnClickInput;
    }
}