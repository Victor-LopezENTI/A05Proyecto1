using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerController playerController;

    [SerializeField] private PlayerMovement playerMovement;

    // Input variables
    [SerializeField] public float moveInput, jumpInput, interactInput; // Input from the player

    private void Awake()
    {
        if (Instance == null) Instance = this;
        playerController = new PlayerController();
        playerMovement = new PlayerMovement();
        playerController.Enable();
    }


    // Horizontal movement input [A | D]
    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    // Jump input [Spacebar]
    public void onJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = context.ReadValue<float>();
        }
        
        if (context.duration >= 1)
        {
            jumpInput = 0;
        }
    }

    // Interaction input [E]
    public void onInteract(InputAction.CallbackContext context)
    {
        interactInput = context.ReadValue<float>();
    }
}