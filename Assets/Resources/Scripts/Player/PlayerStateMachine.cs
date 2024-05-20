using UnityEngine;
using UnityEngine.UI;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine instance { get; private set; }

    public const float DistanceFromGround = 0.64f;

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
    public RaycastHit2D OnGround;
    [SerializeField] public Image chargeBar;
    [SerializeField] public Canvas playerUi;

    public float groundCheckDistance;
    public bool isPaused;
    public bool onSlingshot;
    public bool canMoveInAir = true;

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
        RopingState = new Roping();
    }

    private void OnEnable()
    {
        canMoveInAir = true;
        onSlingshot = false;
        groundCheckDistance = 0.1f;
        ChangeState(JumpingState);
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }

        _currentState?.Update();
    }

    private void FixedUpdate()
    {
        if (isPaused)
        {
            playerRb.bodyType = RigidbodyType2D.Static;
            return;
        }

        playerRb.bodyType = RigidbodyType2D.Dynamic;

        switch (PlayerInput.instance.horizontalInput)
        {
            case > 0:
                transform.localScale = new Vector3(1, 1, 1);
                playerUi.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                break;
            case < 0:
                transform.localScale = new Vector3(-1, 1, 1);
                playerUi.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
                break;
        }

        // Ground detection ray-cast
        Debug.DrawLine(playerRb.position - new Vector2(0, DistanceFromGround),
            playerRb.position + Vector2.down * DistanceFromGround + groundCheckDistance * Vector2.down, Color.red);

        OnGround = Physics2D.Raycast(playerRb.position - new Vector2(0, DistanceFromGround), Vector2.down,
            groundCheckDistance, groundLayer);

        _currentState?.FixedUpdate();
    }

    public static void ChangeState(IPlayerState newState)
    {
        //Debug.Log(_currentState + "----->" + newState);
        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Soul"))
    //     {
    //         SoulSpheresCollector.instance.soulSphereCounter++;
    //         SoulSpheresCollector.instance.sceneSphereCounter++;
    //         Destroy(other.gameObject);
    //     }
    // }
    //
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Slingshot"))
    //     {
    //         onSlingshot = true;
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Slingshot"))
    //     {
    //         onSlingshot = false;
    //     }
    // }

    private void OnDisable()
    {
        _currentState?.OnExit();
    }
}