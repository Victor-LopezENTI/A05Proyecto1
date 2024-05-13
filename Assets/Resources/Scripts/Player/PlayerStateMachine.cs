using System;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] public Image chargeBar;
    [SerializeField] public Canvas playerUi;
    [SerializeField] public ParticleSystem sparks;
    
    public float horizontalInput;
    public float jumpInput;
    public float clickInput;

    public bool onGround;
    public bool onSlingshot;

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
        
        IdleState = new Idle();
        WalkingState = new Walking();
        ChargingJumpState = new ChargingJump();
        JumpingState = new Jumping();
        ChargingSlingshotState = new ChargingSlingshot();
        RopingState = new Roping();
    }

    private void OnEnable()
    {
        ChangeState(IdleState);
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.Raycast(playerRb.position, Vector2.down, DistanceFromGround, LayerMask.GetMask("Platforms"));
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