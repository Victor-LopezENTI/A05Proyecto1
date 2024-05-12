using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class Idle : IPlayerState
{
    public Idle()
    {
        OnEnter();
    }

    public void OnEnter()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInputPerformed;
        InputManager.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
    }

    public void Update()
    {
        PlayerStateMachine.instance.playerAnimator.Play("idle");
    }

    public void FixedUpdate()
    {
        PlayerStateMachine.instance.playerRb.velocity = new Vector2(0f, PlayerStateMachine.instance.playerRb.velocity.y);
        
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

    public void OnExit()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnMovementInputPerformed;
        InputManager.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
    }
}