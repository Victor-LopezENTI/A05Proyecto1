using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Roping : IPlayerState
{
    private const float RopeSpeed = 550f;

    private Rigidbody2D playerRb => PlayerStateMachine.instance.playerRb;
    
    public void OnEnter()
    {
        RopeManager.Instance.LaunchRope(RopeManager.Instance.selectedHook.transform);
        
        PlayerInput.instance.PlayerInputActions.Player.Jump.performed += OnJumpInputPerformed;
        PlayerInput.instance.PlayerInputActions.Player.Click.canceled += OnClickInputCanceled;
    }

    public void Update()
    {
        //RopeManager.Instance.sparks.gameObject.SetActive(PlayerStateMachine.instance.verticalInput != 0);

        // Play animations based on vertical input
        switch (playerRb.velocity.y)
        {
            case > 0:
                PlayerStateMachine.instance.playerAnimator.Play("jump");
                break;
            case < 0:
                PlayerStateMachine.instance.playerAnimator.Play("fall");
                break;
        }

        
        RopeManager.Instance.ropeLineRenderer.SetPosition(0, playerRb.position);
    }

    public void FixedUpdate()
    {
        //RopeManager.Instance.ClimbRope(-PlayerStateMachine.instance.verticalInput);
        
        if (PlayerStateMachine.instance.OnGround)
        {
            RopeManager.Instance.DeselectHook();
            
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
        }
        
        playerRb.AddForce(PlayerInput.instance.horizontalInput * RopeSpeed * Vector2.right);
    }

    private void OnHorizontalInput(InputAction.CallbackContext context)
    {
        playerRb.AddForce(new Vector2(context.ReadValue<float>() * RopeSpeed, 0));
    }
    
    private void OnJumpInputPerformed(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    
    private void OnClickInputCanceled(InputAction.CallbackContext context)
    {
        RopeManager.Instance.DeselectHook();
        PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
    }

    public void OnExit()
    {
        RopeManager.Instance.sparks.gameObject.SetActive(false);
        
        PlayerInput.instance.PlayerInputActions.Player.Jump.performed -= OnJumpInputPerformed;
        PlayerInput.instance.PlayerInputActions.Player.Click.canceled -= OnClickInputCanceled;
    }
}