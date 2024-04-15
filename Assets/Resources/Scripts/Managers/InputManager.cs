using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Singleton Pattern

    private static InputManager _Instance;
    public static InputManager Instance
    {
        get
        {
            if (!_Instance)
            {
                // Load the prefab from the Resources folder
                var prefab = Resources.Load<GameObject>("Prefabs/Managers/InputManager");

                // Instantiate the prefab
                var inScene = Instantiate<GameObject>(prefab);

                // Get the InputManager component from the prefab
                _Instance = inScene.GetComponentInChildren<InputManager>();

                // If the component is not found, add it to the prefab
                if (!_Instance)
                    _Instance = inScene.AddComponent<InputManager>();

                DontDestroyOnLoad(_Instance.transform.root.gameObject);
            }
            return _Instance;
        }
    }

    #endregion

    private PlayerController playerController;

    // Input variables
    public float moveInput { get; private set; }
    public float jumpInput { get; private set; }
    public float interactInput { get; private set; }

    private void Awake()
    {
        // Create and enable the player controller
        playerController = new PlayerController();
        playerController.Enable();
    }

    // Horizontal movement input [A | D]
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!RotationManager.Instance.chamberUpsideDown)
            moveInput = context.ReadValue<float>();
        else
            moveInput = -context.ReadValue<float>();
    }

    // Jump input [Spacebar]
    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.ReadValue<float>();
    }

    // Interaction input [E]
    public void OnInteract(InputAction.CallbackContext context)
    {
        interactInput = context.ReadValue<float>();
    }
}