using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    #region Variables

    PlayerStateMachine playerStateMachine;

    // Player components
    public Rigidbody2D playerRB { get; private set; }
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;
    private SlingshotJump slingshotJump;

    // Input variables
    private float moveInput;
    //private bool canInput;

    // Jump timer variables
    private float holdTimer;
    [SerializeField] private float holdNormTimer;
    private const float maxHoldTime = 0.5f;

    // Movement variables
    public bool facingRight { get; private set; }
    private const float moveSpeed = 400f;

    // Jump variables
    private const float jumpForce = 1800f;
    private const float minJumpForce = 1250f;

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

        // Get components
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        slingshotJump = GetComponent<SlingshotJump>();
    }

    private void Update()
    {
        // Flip the player sprite
        if (moveInput != 0)
        {
            facingRight = moveInput * RotationManager.Instance.globalDirection.x < 0;
            playerSprite.flipX = facingRight;
        }
    }

    private void FixedUpdate()
    {
        // Get the inputs from InputManager
        moveInput = InputManager.Instance.moveInput;

        // Switch all possible PlayerStates
        switch (playerStateMachine.currentState)
        {
            case PlayerStateMachine.PlayerState.Walking:
                playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, 0);
                playerAnimator.Play("walk");
                break;

            case PlayerStateMachine.PlayerState.ChargingJump:
                holdTimer += Time.deltaTime;
                if (holdTimer > maxHoldTime)
                    holdTimer = maxHoldTime;

                holdNormTimer = Mathf.Lerp(0, 1, holdTimer / maxHoldTime);
                playerRB.velocity = new(0f, playerRB.velocity.y * RotationManager.Instance.globalDirection.y);
                playerAnimator.Play("charge_jump");
                break;

            case PlayerStateMachine.PlayerState.StartingJump:
                if (holdTimer < 0.25f)
                    playerRB.AddForce(new(moveInput * moveSpeed * (minJumpForce / jumpForce), minJumpForce * RotationManager.Instance.globalDirection.y));
                else
                    playerRB.AddForce(new(moveInput * moveSpeed * holdNormTimer, jumpForce * holdNormTimer * RotationManager.Instance.globalDirection.y));
                holdTimer = 0f;
                break;

            case PlayerStateMachine.PlayerState.Roping:

                break;

            case PlayerStateMachine.PlayerState.ChargingSlingshot:
                playerRB.velocity = new(0f, playerRB.velocity.y * RotationManager.Instance.globalDirection.y);
                break;

            case PlayerStateMachine.PlayerState.StartingSlingshot:
                playerRB.AddForce(slingshotJump.escapeForce);
                break;

            case PlayerStateMachine.PlayerState.Jumping:
                playerRB.velocity = new(playerRB.velocity.x + moveInput * moveSpeed * Time.deltaTime * RotationManager.Instance.globalDirection.x,
                                        playerRB.velocity.y * RotationManager.Instance.globalDirection.y);
                playerAnimator.Play("jump");
                break;

            case PlayerStateMachine.PlayerState.Falling:
                playerRB.velocity = new(playerRB.velocity.x + moveInput * moveSpeed * Time.deltaTime * RotationManager.Instance.globalDirection.x,
                                        playerRB.velocity.y * RotationManager.Instance.globalDirection.y);
                playerAnimator.Play("fall");
                break;

            case PlayerStateMachine.PlayerState.Idle:
                playerRB.velocity = new(0f, playerRB.velocity.y * RotationManager.Instance.globalDirection.y);
                playerAnimator.Play("idle");
                break;
        }
    }
}