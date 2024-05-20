using UnityEngine;
using UnityEngine.InputSystem;
using Screen = UnityEngine.Device.Screen;
using Vector2 = UnityEngine.Vector2;

public class Idle : IPlayerState
{
    private Rigidbody2D playerRb => PlayerStateMachine.instance.playerRb;

    public void OnEnter()
    {
        playerRb.gravityScale = 0f;
        
        PlayerInput.instance.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInputPerformed; 
        PlayerInput.instance.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
        PlayerInput.instance.PlayerInputActions.Player.Click.performed += OnClickInputPerformed;
    }

    public void Update()
    {
        PlayerStateMachine.instance.playerAnimator.Play("idle");
    }

    public void FixedUpdate()
    {
        playerRb.velocity = Vector2.zero;

        if (PlayerInput.instance.horizontalInput != 0)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
        }
        else if (PlayerInput.instance.jumpInput > 0)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
        }
    }

    private void OnMovementInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
    }

    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.ChangeState(PlayerStateMachine.ChargingJumpState);
    }

    private void OnClickInputPerformed(InputAction.CallbackContext context)
    {
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
        
        PlayerInput.instance.PlayerInputActions.Player.HorizontalMovement.performed -= OnMovementInputPerformed;
        PlayerInput.instance.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
        PlayerInput.instance.PlayerInputActions.Player.Click.performed -= OnClickInputPerformed;
    }
}