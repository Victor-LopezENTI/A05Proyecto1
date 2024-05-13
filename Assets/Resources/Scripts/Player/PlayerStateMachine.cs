using UnityEngine;
using UnityEngine.UI;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine instance { get; private set; }

    public const float DistanceFromGround = 0.85f;

    #region Variables

    private static IPlayerState _currentState;
    public static IPlayerState IdleState;
    public static IPlayerState WalkingState;
    public static IPlayerState ChargingJumpState;
    public static IPlayerState JumpingState;
    public static IPlayerState ChargingSlingshotState;
    public static IPlayerState RopingState;

    public Rigidbody2D playerRb;
    public LineRenderer playerLr;
    public Animator playerAnimator;
    public LayerMask groundLayer;
    [SerializeField] public Image chargeBar;
    [SerializeField] public Canvas playerUi;
    [SerializeField] public ParticleSystem sparks;
    public GameObject selectedHook;

    public float horizontalInput;
    public float jumpInput;
    public float clickInput;

    public bool onGround;
    public bool onSlingshot;

    public bool canMoveInAir;

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
        playerLr = GetComponent<LineRenderer>();
        playerAnimator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Platforms");

        IdleState = new Idle();
        WalkingState = new Walking();
        ChargingJumpState = new ChargingJump();
        JumpingState = new Jumping();
        ChargingSlingshotState = new ChargingSlingshot();
        RopingState = new Roping(this);
    }

    private void OnEnable()
    {
        canMoveInAir = true;
        ChangeState(IdleState);
    }

    private void Update()
    {
        _currentState?.Update();
    }
    
    private void FixedUpdate()
    {
        onGround = Physics2D.Raycast(playerRb.position, Vector2.down, DistanceFromGround,
            LayerMask.GetMask("Platforms"));
        _currentState?.FixedUpdate();
    }

    public static void ChangeState(IPlayerState newState)
    {
        //Debug.Log(_currentState + "----->" + newState);
        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        onSlingshot = other.gameObject.CompareTag("Slingshot");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        onSlingshot = !other.gameObject.CompareTag("Slingshot");
    }

    private void OnDisable()
    {
        _currentState?.OnExit();
    }
}