using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void onMove(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
    }
    public void onJump(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
    }
}