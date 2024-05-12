using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : IPlayerState
{
    private const float WalkSpeed = 100f;
    private static float _horizontalMovement;

    void IPlayerState.OnEnter()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnMovementInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled += OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            PlayerStateMachine.instance.ChangeState(PlayerStateMachine.instance.IdleState);
        }
        else _horizontalMovement = context.ReadValue<float>();
    }

    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.ChangeState(PlayerStateMachine.instance.ChargingJumpState);
    }

    public void Update()
    {
        Debug.Log(_horizontalMovement);
        if (_horizontalMovement > 0)
        {
            Debug.Log("Right");
            PlayerStateMachine.instance.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_horizontalMovement < 0)
        {
            Debug.Log("Left");
            PlayerStateMachine.instance.transform.localScale = new Vector3(-1, 1, 1);
        }

        PlayerStateMachine.instance.playerAnimator.Play("walk");
    }

    public void FixedUpdate()
    {
        PlayerStateMachine.instance.playerRb.velocity =
            new Vector2(_horizontalMovement * WalkSpeed, 0f);
    }

    public void OnExit()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled -= OnMovementInput;
        InputManager.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
    }
}