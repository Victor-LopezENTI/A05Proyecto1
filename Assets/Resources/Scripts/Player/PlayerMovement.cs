using System;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public SoulSpheresCollector souls;

    #region Variables

    PlayerStateMachine playerStateMachine;

    // Player components
    public Rigidbody2D playerRB { get; private set; }
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;
    private SlingshotJump slingshotJump;
    private RopeManager ropeManager;
    [SerializeField] Image chargeBar;
    [SerializeField] Canvas playerUI;
    [SerializeField] ParticleSystem sparks;

    // Input variables
    private float moveInput;
    [SerializeField] private float verticalInput;

    // Jump timer variables
    private float holdTimer;
    [SerializeField] private float holdNormTimer;
    private const float maxHoldTime = 0.5f;

    // Movement variables
    public bool facingRight { get; private set; }
    private float moveSpeed = 400f;
    private const float moveSpeedWalk = 400f;
    private const float moveSpeedChargeJump = 0f;
    private const float moveSpeedJump = 350f;

    // Jump variables
    private const float jumpForce = 1800f;
    private const float minJumpForce = 1250f;

    // Rope jump variables
    [SerializeField] private float leaveRopeForce = 90f;
    [SerializeField] private float maxLeaveRopeForce = 1100f;
    [SerializeField] private Vector2 minLeaveRopeImpulse = new(300f, 390f);

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
        ropeManager = GetComponent<RopeManager>();
        souls = GetComponent<SoulSpheresCollector>();

    }

    private void Start()
    {
        playerRB.velocity = Vector2.zero;
    }

    private void Update()
    {
        // Flip the player sprite
        if (moveInput != 0)
        {
            facingRight = moveInput * RotationManager.instance.globalDirection.x < 0;
            playerSprite.flipX = facingRight;
        }
    }

    private void FixedUpdate()
    {
        // Get the inputs from InputManager
        moveInput = InputManager.Instance.moveInput;
        verticalInput = InputManager.Instance.verticalInput;

        //Debug.Log(playerStateMachine.currentState);

        // Switch all possible PlayerStates
        switch (playerStateMachine.currentState)
        {
            case PlayerStateMachine.PlayerState.Walking:
                sparks.gameObject.SetActive(false);
                playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime, 0);
                playerAnimator.Play("walk");
                break;

            case PlayerStateMachine.PlayerState.ChargingJump:
                playerUI.enabled = true;
                moveSpeed = moveSpeedChargeJump;
                holdTimer += Time.deltaTime;
                if (holdTimer > maxHoldTime)
                    holdTimer = maxHoldTime;

                holdNormTimer = Mathf.Lerp(0, 1, holdTimer / maxHoldTime);
                chargeBar.fillAmount = holdNormTimer;
                playerRB.velocity = new(0f, playerRB.velocity.y * RotationManager.instance.globalDirection.y);
                playerAnimator.Play("charge_jump");
                break;

            case PlayerStateMachine.PlayerState.StartingJump:
                AudioManager.Instance.PlaySFX("Jump");
                playerUI.enabled = false;
                moveSpeed = moveSpeedJump;
                if (holdTimer < 0.25f)
                    playerRB.AddForce(new(moveInput * moveSpeed * (minJumpForce / jumpForce), minJumpForce * RotationManager.instance.globalDirection.y));
                else
                    playerRB.AddForce(new(moveInput * moveSpeed * holdNormTimer, jumpForce * holdNormTimer * RotationManager.instance.globalDirection.y));
                holdTimer = 0f;
                break;

            case PlayerStateMachine.PlayerState.EnteringRope:

                break;

            case PlayerStateMachine.PlayerState.LeavingRope:
                Vector2 impulse = Vector2.zero;
                float angleDifference = Mathf.Min(Mathf.Abs(ropeManager.selectedHookAngle), Mathf.Abs(ropeManager.selectedHookAngle - 180));
                float distanceFactor;
                if (!(ropeManager.selectedHookDistance < 0.4f || angleDifference is > 80f and < 100f))
                {
                    float maxRopeLength = ropeManager.selectedHook.GetComponent<CircleCollider2D>().radius *
                                          ropeManager.selectedHook.transform.lossyScale.x;
                    distanceFactor = ropeManager.selectedHookDistance / maxRopeLength;
                    impulse.x = Mathf.Abs(playerRB.velocity.x * leaveRopeForce * distanceFactor);
                    impulse.y = Mathf.Abs(playerRB.velocity.y * leaveRopeForce * distanceFactor);
                    if (playerRB.velocity.magnitude < 10f)
                    {
                        if (!facingRight)
                        {
                            impulse.x -= minLeaveRopeImpulse.x;
                        }
                        else
                        {
                            impulse.x += minLeaveRopeImpulse.x;
                        }
                        impulse.y += minLeaveRopeImpulse.y;
                    }
                    impulse = Vector2.ClampMagnitude(impulse, maxLeaveRopeForce);
                }
                playerRB.AddForce(impulse);
                break;

            case PlayerStateMachine.PlayerState.Roping:
                if (verticalInput != 0)
                {
                    if (ropeManager.ClimbRope(verticalInput))
                    {
                        sparks.gameObject.SetActive(true);
                    }
                    else
                    {
                        sparks.gameObject.SetActive(false);
                    }
                }
                else
                {
                    sparks.gameObject.SetActive(false);
                }
                break;


            case PlayerStateMachine.PlayerState.ChargingSlingshot:
                playerRB.velocity = new(0f, playerRB.velocity.y * RotationManager.instance.globalDirection.y);
                playerAnimator.Play("charge_jump");
                break;

            case PlayerStateMachine.PlayerState.StartingSlingshot:
                playerRB.AddForce(slingshotJump.escapeForce);
                AudioManager.Instance.PlaySFX("SlingShot");
                break;

            case PlayerStateMachine.PlayerState.JumpingSlingshot:
                playerAnimator.Play("jump");
                break;

            case PlayerStateMachine.PlayerState.FallingSlingshot:
                playerAnimator.Play("fall");
                break;

            case PlayerStateMachine.PlayerState.Jumping:
                playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime,
                                        playerRB.velocity.y * RotationManager.instance.globalDirection.y);
                playerAnimator.Play("jump");
                sparks.gameObject.SetActive(false);
                break;

            case PlayerStateMachine.PlayerState.Falling:
                playerRB.velocity = new(moveInput * moveSpeed * Time.deltaTime,
                                        playerRB.velocity.y * RotationManager.instance.globalDirection.y);
                playerAnimator.Play("fall");
                sparks.gameObject.SetActive(false);
                break;

            case PlayerStateMachine.PlayerState.Idle:
                playerRB.velocity = new(0f, playerRB.velocity.y * RotationManager.instance.globalDirection.y);
                playerAnimator.Play("idle");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnTriggerEnter2D(Collider2D soulSphere)
    {
        if (soulSphere.gameObject.CompareTag("Soul"))
        {
            PlayerPrefs.SetInt("SoulSphere", ++souls.soulSphereCounter);
            Destroy(soulSphere.gameObject);
        }
    }
}