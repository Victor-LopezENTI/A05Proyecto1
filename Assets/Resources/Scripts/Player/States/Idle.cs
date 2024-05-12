using UnityEngine.InputSystem;

public class Idle : IPlayerState
{
    public void OnEnter()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInputPerformed;
        InputManager.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
    }

    private void OnMovementInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.ChangeState(PlayerStateMachine.instance.WalkingState);
    }
    
    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.ChangeState(PlayerStateMachine.instance.ChargingJumpState);
    }

    public void Update()
    {
        PlayerStateMachine.instance.playerAnimator.Play("idle");
    }

    public void FixedUpdate()
    {
    }

    public void OnExit()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnMovementInputPerformed;
        InputManager.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
    }
}