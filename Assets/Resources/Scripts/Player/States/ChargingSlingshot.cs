using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class ChargingSlingshot : IPlayerState
{
	// WIP constants
	private const float MinDragPos = 20f;
	private const float MaxDragPos = 50f;
	private const float SlingshotForce = 50f;
	private static readonly Vector2 SlingshotForceMax = new Vector2(1300f, 2000f);

	private float _slingshotBuffer;
	private static readonly Vector2 DragStartPos = new Vector2(Screen.width / 2, Screen.height / 2);
	private Vector2 _dragEndPos;
	private Vector2 _escapeForce;

	// Line renderer
	private const int Steps = 400;
	private Vector2[] _trajectory;

	public void OnEnter()
	{
		InputManager.PlayerInputActions.Player.Click.performed += OnClick;
		InputManager.PlayerInputActions.Player.Click.canceled += OnClick;
	}

	public void Update()
	{
		PlayerStateMachine.instance.playerAnimator.Play("charge_jump");
		_dragEndPos = Mouse.current.position.ReadValue();

		_slingshotBuffer = (DragStartPos - _dragEndPos).magnitude;

		PlayerStateMachine.instance.playerLr.enabled = _slingshotBuffer >= MinDragPos;
		_slingshotBuffer = Mathf.Clamp(_slingshotBuffer, MinDragPos, MaxDragPos);

		_escapeForce = (DragStartPos - _dragEndPos).normalized * (2f * _slingshotBuffer);
		_escapeForce = new Vector2(Mathf.Clamp(_escapeForce.x, -SlingshotForceMax.x, SlingshotForceMax.x),
			Mathf.Clamp(_escapeForce.y, 0f, SlingshotForceMax.y));

		var angle = Mathf.Atan2(DragStartPos.y - _dragEndPos.y, DragStartPos.x - _dragEndPos.x);
		//  if (angle is >= 0.5f and <= 2.5f)
		// {
		_trajectory = Plot(PlayerStateMachine.instance.playerRb, PlayerStateMachine.instance.playerRb.position,
			_escapeForce);
		DrawTrajectory();
		//  }
	}

	public void FixedUpdate()
	{
	}

	private static Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity)
	{
		var results = new Vector2[Steps];

		var timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
		var gravityAccel = Physics2D.gravity * (rigidbody.gravityScale * timestep * timestep);

		var drag = 1f - timestep * rigidbody.drag;
		var moveStep = velocity * timestep;

		for (int i = 0; i < Steps; i++)
		{
			moveStep += gravityAccel;
			moveStep *= drag;
			pos += moveStep;
			results[i] = pos;
		}

		return results;
	}

	private void DrawTrajectory()
	{
		PlayerStateMachine.instance.playerLr.enabled = true;
		PlayerStateMachine.instance.playerLr.positionCount = _trajectory.Length;

		var positions = new Vector3[_trajectory.Length];
		for (var i = 0; i < _trajectory.Length; i++)
			positions[i] = _trajectory[i];
		PlayerStateMachine.instance.playerLr.SetPositions(positions);
	}

	private void OnClick(InputAction.CallbackContext context)
	{
		PlayerStateMachine.instance.clickInput = context.ReadValue<float>();
		if (context.performed)
		{
			PlayerStateMachine.instance.playerLr.enabled = true;
		}
		else if (context.canceled)
		{
			_escapeForce = (DragStartPos - _dragEndPos).normalized * _slingshotBuffer * 2f * SlingshotForce;
			_escapeForce = new Vector2(Mathf.Clamp(_escapeForce.x, -SlingshotForceMax.x, SlingshotForceMax.x),
				Mathf.Clamp(_escapeForce.y, 0f, SlingshotForceMax.y));
			Debug.Log(_escapeForce);

			PlayerStateMachine.instance.canMoveInAir = false;
			PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
		}
	}

	public void OnExit()
	{
		PlayerStateMachine.instance.playerRb.AddForce(_escapeForce);
		PlayerStateMachine.instance.playerLr.enabled = false;

		_escapeForce = Vector2.zero;
		_dragEndPos = Vector2.zero;
		_slingshotBuffer = 0f;

		InputManager.PlayerInputActions.Player.Click.performed -= OnClick;
		InputManager.PlayerInputActions.Player.Click.canceled -= OnClick;
	}
}