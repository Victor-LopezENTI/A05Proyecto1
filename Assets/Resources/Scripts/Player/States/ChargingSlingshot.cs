using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class ChargingSlingshot : IPlayerState
{
    // Constants
    private const float SlingshotForce = 4.4f;
    private const float MinDragPos = 1000;
    private const int MaxSteps = 400;
    private static readonly Vector2 EscapeForceMax = new(1400f, 2800f);

    private bool _isDragging;
    private Vector2 _vectorToCenter;
    private static readonly Vector2 DragStartPos = new(Screen.width / 2, Screen.height / 2);
    private Vector2 _escapeForce;

    // Line renderer
    private const int Steps = 400;
    private Vector2[] _trajectory;

    public void OnEnter()
    {
        PlayerStateMachine.instance.playerRb.velocity =
            new Vector2(0f, PlayerStateMachine.instance.playerRb.velocity.y);
        PlayerStateMachine.instance.playerLr.enabled = true;
        
        _isDragging = false;

        InputManager.PlayerInputActions.Player.Click.performed += OnClick;
        InputManager.PlayerInputActions.Player.Click.canceled += OnClick;
    }

    public void Update()
    {
        PlayerStateMachine.instance.playerAnimator.Play("charge_jump");

        var dragEndPos = Mouse.current.position.ReadValue();
        _vectorToCenter = DragStartPos - dragEndPos;

        if (_vectorToCenter.magnitude > MinDragPos)
        {
            _isDragging = true;
        }
        else if (_isDragging)
        {
            PlayerStateMachine.instance.playerLr.enabled = false;
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
        }
        
        var angle = Mathf.Atan2(DragStartPos.y - dragEndPos.y, DragStartPos.x - dragEndPos.x);
        if (angle is >= 0.5f and <= 2.5f)
        {
            var vectorToCenter = _vectorToCenter;
            var plotVelocity = vectorToCenter * SlingshotForce / 50f;

            PlayerStateMachine.instance.playerLr.enabled = vectorToCenter.magnitude >= MinDragPos;
            _trajectory = Plot(PlayerStateMachine.instance.playerRb, PlayerStateMachine.instance.playerRb.position,
                plotVelocity);
            DrawTrajectory();
        }
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
            _escapeForce = _vectorToCenter * SlingshotForce;
            AudioManager.Instance.PlaySFX("SlingShot");

            PlayerStateMachine.instance.canMoveInAir = false;
            PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
        }
    }

    public void OnExit()
    {
        PlayerStateMachine.instance.playerRb.AddForce(_escapeForce);
        PlayerStateMachine.instance.playerLr.enabled = false;
        PlayerStateMachine.instance.onSlingshot = false;

        _vectorToCenter = Vector2.zero;
        _escapeForce = Vector2.zero;

        InputManager.PlayerInputActions.Player.Click.performed -= OnClick;
        InputManager.PlayerInputActions.Player.Click.canceled -= OnClick;
    }
}