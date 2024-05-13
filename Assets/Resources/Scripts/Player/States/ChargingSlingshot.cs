using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class ChargingSlingshot : IPlayerState
{
    // WIP constants
    private const float MinDragPos = 5f;
    private const float MaxDragPos = 25f;
    private const float SlingshotForce = 3f;

    private static readonly Vector2 DragStartPos = new Vector2(Screen.width / 2, Screen.height / 2);
    private Vector2 _dragEndPos;
    private float _slingshotBuffer;
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

        _escapeForce = (DragStartPos - _dragEndPos) * SlingshotForce;
        var angle = Mathf.Atan2(DragStartPos.y - _dragEndPos.y, DragStartPos.x - _dragEndPos.x);
      //  if (angle is >= 0.5f and <= 2.5f)
      // {
            _trajectory = new Vector2[Steps];
            _trajectory = Plot(PlayerStateMachine.instance.playerRb, PlayerStateMachine.instance.playerRb.position,
                _escapeForce);
            PlayerStateMachine.instance.playerLr.positionCount = _trajectory.Length;

            var positions = new Vector3[_trajectory.Length];
            for (var i = 0; i < _trajectory.Length; i++)
                positions[i] = _trajectory[i];
            PlayerStateMachine.instance.playerLr.SetPositions(positions);
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

    private void OnClick(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            PlayerStateMachine.instance.playerRb.AddForce(_escapeForce);
            Debug.Log(_escapeForce);
            _escapeForce = Vector2.zero;
            PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
        }

        PlayerStateMachine.instance.clickInput = context.ReadValue<float>();
    }

    public void OnExit()
    {
        InputManager.PlayerInputActions.Player.Click.performed -= OnClick;
        InputManager.PlayerInputActions.Player.Click.canceled -= OnClick;
    }
}