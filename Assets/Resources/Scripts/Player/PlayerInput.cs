using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance { get; private set; }
    public PlayerInputActions PlayerInputActions;

    [SerializeField] private float _horizontalInput;
    [SerializeField] private float _verticalInput;
    [SerializeField] private float _jumpInput;
    [SerializeField] private float _clickInput;
    [SerializeField] private float _resetInput;

    public float horizontalInput => _horizontalInput;
    public float verticalInput => _verticalInput; 
    public float jumpInput => _jumpInput;
    public float clickInput => _clickInput;
    public float resetInput => _resetInput;

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

        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();
    }

    private void OnEnable()
    {
        PlayerInputActions.Player.HorizontalMovement.performed += OnHorizontalInput;
        PlayerInputActions.Player.HorizontalMovement.canceled += OnHorizontalInput;
        PlayerInputActions.Player.VerticalMovement.performed += OnVerticalInput;
        PlayerInputActions.Player.VerticalMovement.canceled += OnVerticalInput;
        PlayerInputActions.Player.Jump.performed += OnJumpInput;
        PlayerInputActions.Player.Jump.canceled += OnJumpInput;
        PlayerInputActions.Player.Click.performed += OnClickInput;
        PlayerInputActions.Player.Click.canceled += OnClickInput;
        PlayerInputActions.Player.Reset.performed += OnResetInput;
        PlayerInputActions.Player.Reset.canceled += OnResetInput;
    }

    private void OnHorizontalInput(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<float>();
    }

    private void OnVerticalInput(InputAction.CallbackContext context)
    {
        _verticalInput = context.ReadValue<float>();
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        _jumpInput = context.ReadValue<float>();
    }

    private void OnClickInput(InputAction.CallbackContext context)
    {
        _clickInput = context.ReadValue<float>();
    }

    private void OnResetInput(InputAction.CallbackContext context)
    {
        _resetInput = context.ReadValue<float>();
    }

    private void OnDisable()
    {
        PlayerInputActions.Player.HorizontalMovement.performed -= OnHorizontalInput;
        PlayerInputActions.Player.HorizontalMovement.canceled -= OnHorizontalInput;
        PlayerInputActions.Player.VerticalMovement.performed -= OnVerticalInput;
        PlayerInputActions.Player.VerticalMovement.canceled -= OnVerticalInput;
        PlayerInputActions.Player.Jump.performed -= OnJumpInput;
        PlayerInputActions.Player.Jump.canceled -= OnJumpInput;
        PlayerInputActions.Player.Click.performed -= OnClickInput;
        PlayerInputActions.Player.Click.canceled -= OnClickInput;
        PlayerInputActions.Player.Reset.performed -= OnResetInput;
        PlayerInputActions.Player.Reset.canceled -= OnResetInput;
    }
}