//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Input/Player Controller.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerController: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Controller"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""d84906de-a92d-47af-ba40-c66fc9b23d7a"",
            ""actions"": [
                {
                    ""name"": ""Movimiento Horizontal"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3523f01e-ba80-4d96-9633-78dedf8cb785"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Movimiento Vertical"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b7c62491-b3ef-4c53-b8b2-556ab9ca50ad"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Salto"",
                    ""type"": ""Button"",
                    ""id"": ""681f3d52-a9f6-4a4c-b8d5-e8df76d50366"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interactuar"",
                    ""type"": ""Button"",
                    ""id"": ""ed398bea-2508-4621-b94f-ed7ed4a24da4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancelar"",
                    ""type"": ""Button"",
                    ""id"": ""a8599679-4216-4604-a539-0080ad8ec294"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mouse Click"",
                    ""type"": ""Button"",
                    ""id"": ""2f503855-f96f-4a09-a4fb-55c1f2879418"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d3e75d8f-e1b1-4b4b-a83d-e88a319a8d34"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimiento Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard X Axis"",
                    ""id"": ""0b8ed2f0-f7b4-4642-87c1-96686f28d62a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimiento Horizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8fdffe9c-4cf1-44cc-a814-bca8cc287c83"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimiento Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9acdcc45-0d1e-41a1-b4b0-81e69b31b132"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimiento Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3fe26068-8829-4aba-aacd-28f849558015"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Salto"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df150cdd-79f4-4bbf-8901-977fc06d531e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Salto"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6184c67-1855-4ec5-afd8-6f90960620f5"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interactuar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4054462f-f5d4-4bfe-a5df-93556e12dd64"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interactuar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdd5db87-3650-4a95-b6b4-26fb288303e7"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancelar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc9ba3e2-fcb9-4296-b6aa-7a105985e1da"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancelar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09ce7892-f4dc-4d4b-abb7-842a3d469916"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75566e2f-6cc3-4de2-811f-8568cd1f00a1"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimiento Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard Y Axis"",
                    ""id"": ""c0ba634a-e5ec-4b15-a5b6-96c6e1101733"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimiento Vertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b3a0cc21-9837-4d08-8e0e-422fe82c012c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimiento Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7614e20d-99d6-4e07-9206-23d012512aa0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimiento Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MovimientoHorizontal = m_Player.FindAction("Movimiento Horizontal", throwIfNotFound: true);
        m_Player_MovimientoVertical = m_Player.FindAction("Movimiento Vertical", throwIfNotFound: true);
        m_Player_Salto = m_Player.FindAction("Salto", throwIfNotFound: true);
        m_Player_Interactuar = m_Player.FindAction("Interactuar", throwIfNotFound: true);
        m_Player_Cancelar = m_Player.FindAction("Cancelar", throwIfNotFound: true);
        m_Player_MouseClick = m_Player.FindAction("Mouse Click", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_MovimientoHorizontal;
    private readonly InputAction m_Player_MovimientoVertical;
    private readonly InputAction m_Player_Salto;
    private readonly InputAction m_Player_Interactuar;
    private readonly InputAction m_Player_Cancelar;
    private readonly InputAction m_Player_MouseClick;
    public struct PlayerActions
    {
        private @PlayerController m_Wrapper;
        public PlayerActions(@PlayerController wrapper) { m_Wrapper = wrapper; }
        public InputAction @MovimientoHorizontal => m_Wrapper.m_Player_MovimientoHorizontal;
        public InputAction @MovimientoVertical => m_Wrapper.m_Player_MovimientoVertical;
        public InputAction @Salto => m_Wrapper.m_Player_Salto;
        public InputAction @Interactuar => m_Wrapper.m_Player_Interactuar;
        public InputAction @Cancelar => m_Wrapper.m_Player_Cancelar;
        public InputAction @MouseClick => m_Wrapper.m_Player_MouseClick;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @MovimientoHorizontal.started += instance.OnMovimientoHorizontal;
            @MovimientoHorizontal.performed += instance.OnMovimientoHorizontal;
            @MovimientoHorizontal.canceled += instance.OnMovimientoHorizontal;
            @MovimientoVertical.started += instance.OnMovimientoVertical;
            @MovimientoVertical.performed += instance.OnMovimientoVertical;
            @MovimientoVertical.canceled += instance.OnMovimientoVertical;
            @Salto.started += instance.OnSalto;
            @Salto.performed += instance.OnSalto;
            @Salto.canceled += instance.OnSalto;
            @Interactuar.started += instance.OnInteractuar;
            @Interactuar.performed += instance.OnInteractuar;
            @Interactuar.canceled += instance.OnInteractuar;
            @Cancelar.started += instance.OnCancelar;
            @Cancelar.performed += instance.OnCancelar;
            @Cancelar.canceled += instance.OnCancelar;
            @MouseClick.started += instance.OnMouseClick;
            @MouseClick.performed += instance.OnMouseClick;
            @MouseClick.canceled += instance.OnMouseClick;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @MovimientoHorizontal.started -= instance.OnMovimientoHorizontal;
            @MovimientoHorizontal.performed -= instance.OnMovimientoHorizontal;
            @MovimientoHorizontal.canceled -= instance.OnMovimientoHorizontal;
            @MovimientoVertical.started -= instance.OnMovimientoVertical;
            @MovimientoVertical.performed -= instance.OnMovimientoVertical;
            @MovimientoVertical.canceled -= instance.OnMovimientoVertical;
            @Salto.started -= instance.OnSalto;
            @Salto.performed -= instance.OnSalto;
            @Salto.canceled -= instance.OnSalto;
            @Interactuar.started -= instance.OnInteractuar;
            @Interactuar.performed -= instance.OnInteractuar;
            @Interactuar.canceled -= instance.OnInteractuar;
            @Cancelar.started -= instance.OnCancelar;
            @Cancelar.performed -= instance.OnCancelar;
            @Cancelar.canceled -= instance.OnCancelar;
            @MouseClick.started -= instance.OnMouseClick;
            @MouseClick.performed -= instance.OnMouseClick;
            @MouseClick.canceled -= instance.OnMouseClick;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMovimientoHorizontal(InputAction.CallbackContext context);
        void OnMovimientoVertical(InputAction.CallbackContext context);
        void OnSalto(InputAction.CallbackContext context);
        void OnInteractuar(InputAction.CallbackContext context);
        void OnCancelar(InputAction.CallbackContext context);
        void OnMouseClick(InputAction.CallbackContext context);
    }
}
