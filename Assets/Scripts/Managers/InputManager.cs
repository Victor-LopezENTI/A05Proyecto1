using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerController playerController;

    // Input variables
    public float moveInput, jumpInput, interactInput;

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
    public void OnMove(InputAction.CallbackContext context)
    {
        Instance.moveInput = context.ReadValue<float>();
    }

    // Jump input [Spacebar]
    public void OnJump(InputAction.CallbackContext context)
    {
        Instance.jumpInput = context.ReadValue<float>();
    }

    // Interaction input [E]
    public void OnInteract(InputAction.CallbackContext context)
    {
        Instance.interactInput = context.ReadValue<float>();
    }
}