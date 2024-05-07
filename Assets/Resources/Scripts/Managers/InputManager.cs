using System;
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
    public float verticalInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool interactInput { get; private set; }
    public bool clickInput { get; private set; }
    public bool clickReleased { get; private set; }

    private void Awake()
    {
        // Create and enable the player controller
        playerController = new PlayerController();
        playerController.Enable();
    }

    // Horizontal movement input [A | D]
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>() * RotationManager.Instance.globalDirection.x;
    }

    // Vertical movement input [W | S]
    public void OnVertical(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<float>() * RotationManager.Instance.globalDirection.y;
    }

    // Jump input [Spacebar]
    public void OnJump(InputAction.CallbackContext context)
    {
        float fJumpInput = context.ReadValue<float>();
        jumpInput = fJumpInput != 0;
    }

    // Interaction input [E]
    public void OnInteract(InputAction.CallbackContext context)
    {
        float fInteractInput = context.ReadValue<float>();
        interactInput = fInteractInput != 0;
    }

    // Action input [LMB]
    public void OnClick(InputAction.CallbackContext context)
    {
        float fClickInput = context.ReadValue<float>();
        clickInput = fClickInput != 0;

        clickReleased = context.canceled;
    }
}