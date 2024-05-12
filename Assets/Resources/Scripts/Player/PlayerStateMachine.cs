using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine instance { get; private set; }
    
    #region Variables

    private static IPlayerState _currentState;
    public static IPlayerState IdleState;
    public static IPlayerState WalkingState;
    public static IPlayerState ChargingJumpState;
    public static IPlayerState JumpingState;
    
    public Rigidbody2D playerRb;
    public Animator playerAnimator;
    
    public float horizontalInput;
    public float jumpInput;

    #endregion
    
    private void Awake()
    {
        #region Singleton Pattern

        if (instance != null)
        {
            Debug.Log("There is already an instance of " + instance);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        #endregion

        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        
        IdleState = new Idle();
        WalkingState = new Walking();
        ChargingJumpState = new ChargingJump();
        JumpingState = new Jumping();
        
        ChangeState(IdleState);
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public static void ChangeState(IPlayerState newState)
    {
        //Debug.Log(_currentState + "----->" + newState);
        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
    }
}

/*
private void FixedUpdate()
{
    // Ground check
    onGround = Physics2D.Raycast(transform.position, Vector2.down * RotationManager.instance.globalDirection, distanceFromGround, groundLayer);

    // Get the inputs from InputManager
    moveInput = InputManager.instance.moveInput;
    jumpInput = InputManager.instance.jumpInput;
    clickInput = InputManager.instance.clickInput;
    if (isPaused)
    {
        currentState = PlayerState.Paused;
    }
    else if (onGround)
    {
        // JumpingSlingshot
        if (lastState == PlayerState.StartingSlingshot)
            currentState = PlayerState.JumpingSlingshot;

        // ChargingJump
        else if (jumpInput && !clickInput)
            currentState = PlayerState.ChargingJump;

        // ChargingSlingshot
        else if (slingshotJump.chargingSlingshot)
            currentState = PlayerState.ChargingSlingshot;

        // StartingJump
        else if (lastState == PlayerState.ChargingJump)
            currentState = PlayerState.StartingJump;

        // StartingSlingshot
        else if (slingshotJump.startSlingshot)
            currentState = PlayerState.StartingSlingshot;

        // Walking
        else if (moveInput != 0)
            currentState = PlayerState.Walking;

        // Idle
        else if (moveInput == 0 && lastState != PlayerState.Jumping)
            currentState = PlayerState.Idle;
    }
    else
    {
        // EnteringRope
        if (ropeManager.enteringRope)
            currentState = PlayerState.EnteringRope;

        // LeavingRope
        else if (ropeManager.leavingRope)
            currentState = PlayerState.LeavingRope;

        // Roping
        else if (currentState != PlayerState.EnteringRope && currentState != PlayerState.LeavingRope && ropeManager.hingeConnected)
            currentState = PlayerState.Roping;

        else if (PlayerMovement.Instance.playerRB.velocity.y * RotationManager.instance.globalDirection.y >= 0)
        {
            // JumpingSlingshot
            if (slingshotJump.jumpingSlingshot)
                currentState = PlayerState.JumpingSlingshot;

            // Jumping
            else
                currentState = PlayerState.Jumping;
        }
        else if (PlayerMovement.Instance.playerRB.velocity.y * RotationManager.instance.globalDirection.y < 0)
        {
            // FallingSlingshot
            if (slingshotJump.jumpingSlingshot)
                currentState = PlayerState.FallingSlingshot;

            // Falling
            else
                currentState = PlayerState.Falling;
        }
    }
    lastState = currentState;
}
*/