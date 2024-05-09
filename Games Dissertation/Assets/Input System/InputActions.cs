//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Input System/InputActions.inputactions
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

public partial class @InputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""e583ee89-8583-4be2-a0ef-6ac325d1546f"",
            ""actions"": [
                {
                    ""name"": ""MousePan"",
                    ""type"": ""PassThrough"",
                    ""id"": ""da36c068-fb97-466d-aa1e-675b7633e6fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseZoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d7941c1a-b89b-4098-95cc-a34571cfde8e"",
                    ""expectedControlType"": ""Delta"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseSelect"",
                    ""type"": ""Button"",
                    ""id"": ""df5f0c12-875f-41bd-9aa8-7c24ba7e62df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TouchPan"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3deca3ad-f242-4e9b-b77c-1826076ae475"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TouchZoom"",
                    ""type"": ""Value"",
                    ""id"": ""4d08592d-e4f9-458b-a960-898fe2789e9b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""TouchSelect"",
                    ""type"": ""PassThrough"",
                    ""id"": ""69c952fd-115a-477e-b850-08aa11761711"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Mouse"",
                    ""id"": ""69a65e8f-08ba-4389-b1d1-5da5a9c346a2"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePan"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""aa6386c8-a0b1-42a3-9dd1-d730e28e83e4"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""MousePan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""b0bc1b06-8135-44fd-a8fd-5200a1dd55bc"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""MousePan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7e8a498e-d9f7-4b12-add3-4caeda3835fd"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""TouchPan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a77e802f-9dde-4d4b-9d7d-e6b9ae97d540"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""TouchZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""842030bd-4587-49b5-99a4-cf66fad4afa2"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""MouseZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51b0c0d6-fd82-409b-8bff-6597ff272f09"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""TouchSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""730e3c6b-e5b3-4b67-9592-ad638d7db84a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""MouseSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MousePan = m_Player.FindAction("MousePan", throwIfNotFound: true);
        m_Player_MouseZoom = m_Player.FindAction("MouseZoom", throwIfNotFound: true);
        m_Player_MouseSelect = m_Player.FindAction("MouseSelect", throwIfNotFound: true);
        m_Player_TouchPan = m_Player.FindAction("TouchPan", throwIfNotFound: true);
        m_Player_TouchZoom = m_Player.FindAction("TouchZoom", throwIfNotFound: true);
        m_Player_TouchSelect = m_Player.FindAction("TouchSelect", throwIfNotFound: true);
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
    private readonly InputAction m_Player_MousePan;
    private readonly InputAction m_Player_MouseZoom;
    private readonly InputAction m_Player_MouseSelect;
    private readonly InputAction m_Player_TouchPan;
    private readonly InputAction m_Player_TouchZoom;
    private readonly InputAction m_Player_TouchSelect;
    public struct PlayerActions
    {
        private @InputActions m_Wrapper;
        public PlayerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePan => m_Wrapper.m_Player_MousePan;
        public InputAction @MouseZoom => m_Wrapper.m_Player_MouseZoom;
        public InputAction @MouseSelect => m_Wrapper.m_Player_MouseSelect;
        public InputAction @TouchPan => m_Wrapper.m_Player_TouchPan;
        public InputAction @TouchZoom => m_Wrapper.m_Player_TouchZoom;
        public InputAction @TouchSelect => m_Wrapper.m_Player_TouchSelect;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @MousePan.started += instance.OnMousePan;
            @MousePan.performed += instance.OnMousePan;
            @MousePan.canceled += instance.OnMousePan;
            @MouseZoom.started += instance.OnMouseZoom;
            @MouseZoom.performed += instance.OnMouseZoom;
            @MouseZoom.canceled += instance.OnMouseZoom;
            @MouseSelect.started += instance.OnMouseSelect;
            @MouseSelect.performed += instance.OnMouseSelect;
            @MouseSelect.canceled += instance.OnMouseSelect;
            @TouchPan.started += instance.OnTouchPan;
            @TouchPan.performed += instance.OnTouchPan;
            @TouchPan.canceled += instance.OnTouchPan;
            @TouchZoom.started += instance.OnTouchZoom;
            @TouchZoom.performed += instance.OnTouchZoom;
            @TouchZoom.canceled += instance.OnTouchZoom;
            @TouchSelect.started += instance.OnTouchSelect;
            @TouchSelect.performed += instance.OnTouchSelect;
            @TouchSelect.canceled += instance.OnTouchSelect;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @MousePan.started -= instance.OnMousePan;
            @MousePan.performed -= instance.OnMousePan;
            @MousePan.canceled -= instance.OnMousePan;
            @MouseZoom.started -= instance.OnMouseZoom;
            @MouseZoom.performed -= instance.OnMouseZoom;
            @MouseZoom.canceled -= instance.OnMouseZoom;
            @MouseSelect.started -= instance.OnMouseSelect;
            @MouseSelect.performed -= instance.OnMouseSelect;
            @MouseSelect.canceled -= instance.OnMouseSelect;
            @TouchPan.started -= instance.OnTouchPan;
            @TouchPan.performed -= instance.OnTouchPan;
            @TouchPan.canceled -= instance.OnTouchPan;
            @TouchZoom.started -= instance.OnTouchZoom;
            @TouchZoom.performed -= instance.OnTouchZoom;
            @TouchZoom.canceled -= instance.OnTouchZoom;
            @TouchSelect.started -= instance.OnTouchSelect;
            @TouchSelect.performed -= instance.OnTouchSelect;
            @TouchSelect.canceled -= instance.OnTouchSelect;
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
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMousePan(InputAction.CallbackContext context);
        void OnMouseZoom(InputAction.CallbackContext context);
        void OnMouseSelect(InputAction.CallbackContext context);
        void OnTouchPan(InputAction.CallbackContext context);
        void OnTouchZoom(InputAction.CallbackContext context);
        void OnTouchSelect(InputAction.CallbackContext context);
    }
}
