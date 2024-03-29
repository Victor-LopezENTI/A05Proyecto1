using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerController playerController;

    // The variables the player affect
    private float moveInput, jumpInput, interactInput;

    private void Awake()
    {
        #region Singleton Pattern

        if (Instance != null)
        {
            Debug.Log("There is already an instance of " + Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        #endregion

        // Create and enable the player controller
        playerController = new PlayerController();
        playerController.Enable();
    }

    // Horizontal movement input [A | D]
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    // Jump input [Spacebar]
    private void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.ReadValue<float>();
    }

    // Interaction input [E]
    private void OnInteract(InputAction.CallbackContext context)
    {
        interactInput = context.ReadValue<float>();
    }

    // Public getters
    public float getMoveInput() { return moveInput; }
    public float getJumpInput() { return jumpInput; }
    public float getInteractInput() { return interactInput; }
}