using UnityEngine.InputSystem;

public class ChargingJump : IPlayerState
{
    public void OnEnter()
    {
        InputManager.PlayerInputActions.Player.Jump.canceled += OnJumpInputCanceled;
    }
    
    private void OnJumpInputCanceled(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.ChangeState(PlayerStateMachine.instance.JumpingState);
    }

    public void Update()
    {
        PlayerStateMachine.instance.playerAnimator.Play("charge_jump");
    }

    public void FixedUpdate()
    {
    }

    public void OnExit()
    {
        InputManager.PlayerInputActions.Player.Jump.canceled -= OnJumpInputCanceled;
    }
}
