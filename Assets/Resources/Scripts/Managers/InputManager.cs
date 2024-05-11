using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    #region Events

    public static event Action MovementStarted;
    public static event Action<Vector2> MovementPerformed;
    public static event Action MovementCanceled;
    public static event Action JumpPerformed;
    public static event Action InteractPerformed;
    public static event Action ResetPerformed;
    public static event Action ResetCanceled;
    public static event Action ClickPerformed;

    #endregion

    static InputManager()
    {
        var playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Player.Movement.started += OnMove;
        playerInputActions.Player.Movement.canceled += OnMove;
        playerInputActions.Player.Movement.performed += OnMove;
        playerInputActions.Player.Jump.performed += OnJump;
        playerInputActions.Player.Interact.performed += OnInteract;
        playerInputActions.Player.Reset.performed += OnReset;
        playerInputActions.Player.Click.performed += OnClick;
    }

    private static void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MovementStarted?.Invoke();
        }
        else if (context.performed)
        {
            MovementPerformed?.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.canceled)
        {
            MovementCanceled?.Invoke();
        }
    }

    private static void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpPerformed?.Invoke();
        }
    }

    private static void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractPerformed?.Invoke();
        }
    }

    private static void OnReset(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ResetPerformed?.Invoke();
        }
        else if (context.canceled)
        {
            ResetCanceled?.Invoke();
        }
    }

    private static void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ClickPerformed?.Invoke();
        }
    }
}