using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    Vector2 auxVelocity;
    private void Awake()
    {
        
    }
    public float horizontalMovement(InputAction.CallbackContext context)
    {
        return context.ReadValue<float>();
    }
}
