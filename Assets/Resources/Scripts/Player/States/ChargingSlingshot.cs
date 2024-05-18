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

    // Private variable
    private readonly LayerMask _platformsLayer = LayerMask.GetMask("Platforms");
    private int _currentSteps;
    private bool _isDragging;
    private Vector2 _vectorToCenter;
    private static readonly Vector2 DragStartPos = new(Screen.width / 2, Screen.height / 2);
    private Vector2 _escapeForce;
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
        if (angle is >= 0.5f and <= Mathf.PI - 0.5f)
        {
            var vectorToCenter = _vectorToCenter;
            var plotVelocity = vectorToCenter * SlingshotForce;
            plotVelocity = new Vector2(Mathf.Clamp(plotVelocity.x, -EscapeForceMax.x, EscapeForceMax.x),
                Mathf.Clamp(plotVelocity.y, 0f, EscapeForceMax.y));

            plotVelocity /= 51.7f;

            PlayerStateMachine.instance.playerLr.enabled = vectorToCenter.magnitude >= MinDragPos;
            _trajectory = Plot(PlayerStateMachine.instance.playerRb, PlayerStateMachine.instance.playerRb.position,
                plotVelocity);
            DrawTrajectory();
        }
    }

    public void FixedUpdate()
    {
    }

    private Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity)
    {
        var results = new Vector2[MaxSteps];
        var timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        var gravityAccel = Physics2D.gravity * (rigidbody.gravityScale * timestep * timestep);

        var drag = 1f - timestep * rigidbody.drag;
        var moveStep = velocity * timestep;

        _currentSteps = MaxSteps;
        for (var i = 0; i < MaxSteps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;

            results[i] = pos;

            if (i == 0) continue;
            
            Debug.DrawLine(results[i - 1], results[i], Color.green);
            if (Physics2D.Raycast(results[i - 1], results[i], 0.1f, _platformsLayer))
            {
                _currentSteps = i;
                break;
            }
        }

        return results;
    }

    private void DrawTrajectory()
    {
        PlayerStateMachine.instance.playerLr.enabled = true;
        PlayerStateMachine.instance.playerLr.positionCount = _currentSteps;

        var positions = new Vector3[_currentSteps];
        for (var i = 0; i < _currentSteps; i++)
            positions[i] = _trajectory[i];
        PlayerStateMachine.instance.playerLr.SetPositions(positions);
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        PlayerStateMachine.instance.clickInput = context.ReadValue<float>();
        if (context.performed)
        {
            PlayerStateMachine.instance.playerLr.enabled = true;
            PlayerStateMachine.instance.playerLr.endWidth = 10f;
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
        _escapeForce = new Vector2(Mathf.Clamp(_escapeForce.x, -EscapeForceMax.x, EscapeForceMax.x),
            Mathf.Clamp(_escapeForce.y, 0f, EscapeForceMax.y));
        PlayerStateMachine.instance.playerRb.AddForce(_escapeForce);
        PlayerStateMachine.instance.onSlingshot = false;
        PlayerStateMachine.instance.playerLr.positionCount = 0;
        PlayerStateMachine.instance.playerLr.enabled = false;

        _currentSteps = 0;
        _vectorToCenter = Vector2.zero;
        _escapeForce = Vector2.zero;

        InputManager.PlayerInputActions.Player.Click.performed -= OnClick;
        InputManager.PlayerInputActions.Player.Click.canceled -= OnClick;
    }
}