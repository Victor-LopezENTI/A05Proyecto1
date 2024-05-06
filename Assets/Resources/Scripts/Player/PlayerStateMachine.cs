using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance { get; private set; }

    public enum PlayerState
    {
        Idle,
        Walking,
        ChargingJump,
        StartingJump,
        Jumping,
        Falling,

        // Slingshot states
        ChargingSlingshot,
        StartingSlingshot,
        JumpingSlingshot,
        FallingSlingshot,

        // Rope States
        Roping
    };

    #region Variables

    // Player states variables
    [SerializeField] private PlayerState m_currentState;
    public PlayerState currentState { get => m_currentState; private set => m_currentState = value; }
    private PlayerState lastState;

    // Player input variables
    private float moveInput;
    private bool jumpInput;
    private bool clickInput;

    // Groundcheck variables
    [SerializeField] private LayerMask groundLayer;
    private const float distanceFromGround = 1f;
    [SerializeField] private bool m_onGround;
    public bool onGround { get => m_onGround; private set => m_onGround = value; }

    // Slingshot variables
    private SlingshotJump slingshotJump;

    #endregion

    private void Awake()
    {
        #region Singleton Pattern

        if (Instance != null)
        {
            Debug.Log("There is already an instance of " + Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        #endregion

        slingshotJump = GetComponent<SlingshotJump>();
    }

    private void FixedUpdate()
    {
        // Groundcheck
        onGround = Physics2D.Raycast(transform.position, Vector2.down * RotationManager.Instance.globalDirection, distanceFromGround, groundLayer);

        // Get the inputs from InputManager
        moveInput = InputManager.Instance.moveInput;
        jumpInput = InputManager.Instance.jumpInput;
        clickInput = InputManager.Instance.clickInput;

        if (onGround)
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
            // Roping
            if (GetComponent<RopeManager>().hingeConnected)
                currentState = PlayerState.Roping;

            else if (PlayerMovement.Instance.playerRB.velocity.y * RotationManager.Instance.globalDirection.y >= 0)
            {
                // JumpingSlingshot
                if (slingshotJump.jumpingSlingshot)
                    currentState = PlayerState.JumpingSlingshot;

                // Jumping
                else
                    currentState = PlayerState.Jumping;
            }
            else if (PlayerMovement.Instance.playerRB.velocity.y * RotationManager.Instance.globalDirection.y < 0)
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
}