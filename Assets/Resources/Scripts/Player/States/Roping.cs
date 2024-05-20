using UnityEngine;
using UnityEngine.InputSystem;

public class Roping : IPlayerState
{
    private const float RopeSpeed = 550f;

    private readonly Rigidbody2D _playerRb = PlayerStateMachine.instance.playerRb;
    
    public void OnEnter()
    {
        RopeManager.Instance.LaunchRope(RopeManager.Instance.selectedHook.transform);
        
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnHorizontalInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled += OnHorizontalInput;
        InputManager.PlayerInputActions.Player.VerticalMovement.performed += OnVerticalInput;
        InputManager.PlayerInputActions.Player.VerticalMovement.canceled += OnVerticalInput;
        InputManager.PlayerInputActions.Player.Click.canceled += OnClickCanceled;
    }

    public void Update()
    {
        // Flip sprite based on horizontal input
        switch (PlayerStateMachine.instance.horizontalInput)
        {
            case > 0:
                PlayerStateMachine.instance.transform.localScale = new Vector3(1, 1, 1);
                break;
            case < 0:
                PlayerStateMachine.instance.transform.localScale = new Vector3(-1, 1, 1);
                break;
        }

        RopeManager.Instance.sparks.gameObject.SetActive(PlayerStateMachine.instance.verticalInput != 0);

        // Play animations based on vertical input
        switch (_playerRb.velocity.y)
        {
            case > 0:
                PlayerStateMachine.instance.playerAnimator.Play("jump");
                break;
            case < 0:
                PlayerStateMachine.instance.playerAnimator.Play("fall");
                break;
        }

        
        RopeManager.Instance.ropeLineRenderer.SetPosition(0, _playerRb.position);
    }

    public void FixedUpdate()
    {
        RopeManager.Instance.ClimbRope(-PlayerStateMachine.instance.verticalInput);
        
        if (PlayerStateMachine.instance.OnGround)
        {
            RopeManager.Instance.DeselectHook();
            
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
        }
    }

    private void OnHorizontalInput(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.horizontalInput = context.ReadValue<float>();
        _playerRb.AddForce(new Vector2(context.ReadValue<float>() * RopeSpeed, 0));
    }

    private void OnVerticalInput(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.verticalInput = context.ReadValue<float>();
    }

    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        RopeManager.Instance.DeselectHook();
        PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
    }

    public void OnExit()
    {
        RopeManager.Instance.sparks.gameObject.SetActive(false);
        PlayerStateMachine.instance.verticalInput = 0;
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnHorizontalInput;
        InputManager.PlayerInputActions.Player.HorizontalMovement.canceled -= OnHorizontalInput;
        InputManager.PlayerInputActions.Player.VerticalMovement.performed -= OnVerticalInput;
        InputManager.PlayerInputActions.Player.VerticalMovement.canceled -= OnVerticalInput;
        InputManager.PlayerInputActions.Player.Click.canceled -= OnClickCanceled;
    }
}