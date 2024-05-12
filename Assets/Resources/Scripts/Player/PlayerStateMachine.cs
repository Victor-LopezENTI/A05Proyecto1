using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] public Image chargeBar;
    [SerializeField] public Canvas playerUi;
    [SerializeField] public ParticleSystem sparks;
    
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