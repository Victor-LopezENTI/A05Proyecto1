using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void onMove(InputAction.CallbackContext context)
    {
        context.ReadValue<int>();
    }
    public void onJump(InputAction.CallbackContext context)
    {
        context.ReadValue<bool>();
    }
}
