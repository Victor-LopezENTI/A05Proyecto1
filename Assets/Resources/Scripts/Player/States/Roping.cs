using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Roping : IPlayerState
{
	private readonly Rigidbody2D _playerRb = PlayerStateMachine.instance.playerRb;

	public void OnEnter()
	{
		RopeManager.Instance.ropeLineRenderer.SetPosition(0, _playerRb.position);
		RopeManager.Instance.LaunchRope(RopeManager.Instance.selectedHook.transform);
		
		InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnHorizontalInput;
		InputManager.PlayerInputActions.Player.VerticalMovement.performed += OnVerticalInput;
		InputManager.PlayerInputActions.Player.Click.canceled += OnClickCanceled;
	}

	public void Update()
	{
		Debug.Log("Roping State");
		if (_playerRb.velocity.y > 0)
		{
			PlayerStateMachine.instance.playerAnimator.Play("jump");
		}
		else if (_playerRb.velocity.y < 0)
		{
			PlayerStateMachine.instance.playerAnimator.Play("fall");
		}
	}

	public void FixedUpdate()
	{
		if (!PlayerStateMachine.instance.onGround)
		{
			
		}
		else
		{
			PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
		}
	}

	private void OnHorizontalInput(InputAction.CallbackContext context)
	{
		
	}

	private void OnVerticalInput(InputAction.CallbackContext context)
	{
		
	}

	private void OnClickCanceled(InputAction.CallbackContext context)
	{
		PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
	}
	
	public void OnExit()
	{
		RopeManager.Instance.DestroyRope();
		RopeManager.Instance.selectedHook.GetComponent<Rigidbody2D>().angularVelocity = 0f;
		RopeManager.Instance.selectedHook.GetComponent<Rigidbody2D>().rotation = 0f;
		
		InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnHorizontalInput;
		InputManager.PlayerInputActions.Player.VerticalMovement.performed -= OnVerticalInput;
		InputManager.PlayerInputActions.Player.Click.canceled -= OnClickCanceled;
	}
}