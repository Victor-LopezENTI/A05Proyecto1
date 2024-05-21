using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : IPlayerState
{
    // Constants
    private const float HorizontalSpeed = 9f;

    // Properties
    private Rigidbody2D playerRb => PlayerStateMachine.instance.playerRb;
    private float _timeInAir;

    public void OnEnter()
    {
        PlayerInput.instance.PlayerInputActions.Player.Click.performed += OnClickInputPerformed;
    }

    public void Update()
    {
        if (playerRb.velocity.y > 0)
        {
            PlayerStateMachine.instance.playerAnimator.Play("jump");
        }
        else if (playerRb.velocity.y < 0)
        {
            PlayerStateMachine.instance.playerAnimator.Play("fall");
        }
    }

    public void FixedUpdate()
    {
        // Horizontal movement
        if (PlayerStateMachine.instance.canMoveInAir)
        {
            playerRb.velocity =
                new Vector2(PlayerInput.instance.horizontalInput * HorizontalSpeed, playerRb.velocity.y);
        }

        // Ground detection
        _timeInAir += Time.deltaTime;
        if (_timeInAir > 0.1f)
        {
            if (PlayerStateMachine.instance.OnGround && playerRb.velocity.y <= 1f)
            {
                if (PlayerInput.instance.horizontalInput == 0f)
                {
                    PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
                }
                else
                {
                    PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);
                }
            }
        }
    }

    private void OnClickInputPerformed(InputAction.CallbackContext context)
    {
        if (RopeManager.Instance.selectedHook)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.RopingState);
        }
    }

    public void OnExit()
    {
        _timeInAir = 0;

        PlayerStateMachine.instance.canMoveInAir = true;

        PlayerInput.instance.PlayerInputActions.Player.Click.performed -= OnClickInputPerformed;
    }
}