using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class ChargingSlingshot : IPlayerState
{
    // Constants
    private const float SlingshotForce = 4.4f;
    private const float MinDragPos = 1000;
    private const int MaxSteps = 400;
    private static readonly Vector2 EscapeForceMax = new(1550f, 2400f);

    private BottomHooksBehaviour _bottomHooksBehaviour;
    private bool _isDragging;
    private Vector2 _vectorToCenter;
    private static readonly Vector2 DragStartPos = new(Screen.width / 2, Screen.height / 2);
    private Vector2 _escapeForce;

    // Line renderer
    private const int Steps = 400;
    private Vector2[] _trajectory;

    public void OnEnter()
    {
        _bottomHooksBehaviour = PlayerStateMachine.instance.slingshot.GetComponent<BottomHooksBehaviour>();
        Mouse.current.WarpCursorPosition(DragStartPos);
        PlayerStateMachine.instance.playerRb.velocity =
            new Vector2(0f, PlayerStateMachine.instance.playerRb.velocity.y);
        PlayerStateMachine.instance.playerLr.enabled = true;
        PlayerStateMachine.instance.canMoveInAir = false;

        _isDragging = false;
        PlayerStateMachine.instance.playerLr.positionCount = 0;

        PlayerInput.instance.PlayerInputActions.Player.Click.canceled += OnClickCanceled;
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

        // BottomHooksBehaviour animation

        var angle = Mathf.Atan2(DragStartPos.y - dragEndPos.y, DragStartPos.x - dragEndPos.x);
        if (angle is >= 0.2f and <= Mathf.PI - 0.2f)
        {
            PlayerStateMachine.instance.slingshot.GetComponent<BottomHooksBehaviour>()
                .ChargeJumpAnimation(_vectorToCenter);
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

    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        _escapeForce = _vectorToCenter * SlingshotForce;
        _escapeForce = new Vector2(Mathf.Clamp(_escapeForce.x, -EscapeForceMax.x, EscapeForceMax.x),
            Mathf.Clamp(_escapeForce.y, 0f, EscapeForceMax.y));

        AudioManager.Instance.PlaySFX("SlingShot");
        PlayerStateMachine.instance.canMoveInAir = false;
        PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
    }

    public void OnExit()
    {
        PlayerStateMachine.instance.playerRb.AddForce(_escapeForce);
        PlayerStateMachine.instance.playerLr.enabled = false;
        PlayerStateMachine.instance.slingshot = null;

        _bottomHooksBehaviour.highlight.transform.DOMove(_bottomHooksBehaviour.transform.position, 0.2f);
        _bottomHooksBehaviour = null;
        _vectorToCenter = Vector2.zero;
        _escapeForce = Vector2.zero;

        PlayerInput.instance.PlayerInputActions.Player.Click.canceled -= OnClickCanceled;
    }
}