using UnityEngine;
using UnityEngine.InputSystem;

public class Roping : IPlayerState
{
	private const float RopeSpeed = 400f;
	
	private readonly Rigidbody2D _playerRb = PlayerStateMachine.instance.playerRb;
	private Rigidbody2D _hookRb;
	
	private Vector2 _savedPos;
	

	public void OnEnter()
	{
		RopeManager.Instance.LaunchRope(RopeManager.Instance.selectedHook.transform);
		
		_hookRb = RopeManager.Instance.selectedHook.GetComponent<Rigidbody2D>();
		_savedPos = _hookRb.position;
		
		InputManager.PlayerInputActions.Player.HorizontalMovement.performed += OnHorizontalInput;
		InputManager.PlayerInputActions.Player.VerticalMovement.performed += OnVerticalInput;
		InputManager.PlayerInputActions.Player.Click.canceled += OnClickCanceled;
	}

	public void Update()
	{
		Debug.Log("Roping State");
		
		switch (PlayerStateMachine.instance.horizontalInput)
		{
			case > 0:
				PlayerStateMachine.instance.transform.localScale = new Vector3(1, 1, 1);
				break;
			case < 0:
				PlayerStateMachine.instance.transform.localScale = new Vector3(-1, 1, 1);
				break;
		}
		
		RopeManager.Instance.ropeLineRenderer.SetPosition(0, _playerRb.position);
		
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
			_hookRb.position = _savedPos;
		}
		else
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
		
	}

	private void OnClickCanceled(InputAction.CallbackContext context)
	{
		RopeManager.Instance.DeselectHook();
		PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
	}
	
	public void OnExit()
	{
		_hookRb = null;
		
		InputManager.PlayerInputActions.Player.HorizontalMovement.performed -= OnHorizontalInput;
		InputManager.PlayerInputActions.Player.VerticalMovement.performed -= OnVerticalInput;
		InputManager.PlayerInputActions.Player.Click.canceled -= OnClickCanceled;
	}
}