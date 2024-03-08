using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerController playerController;

    // Input variables
    [SerializeField] public float moveInput, jumpInput, interacted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        playerController = new PlayerController();
        playerController.Enable();
    }

    private void Update()
    {
        // Track the inputs
        moveInput = playerController.Player.MovimientoHorizontal.ReadValue<float>();
        jumpInput = playerController.Player.Salto.ReadValue<float>();
        interacted = playerController.Player.Interactuar.ReadValue<float>();
    }

    // Horizontal movement input [A | D]
    public void onMove(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
    }

    // Jump input [Spacebar]
    public void onJump(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
    }

    // Interaction input [E]
    public void onInteract(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
    }
}
