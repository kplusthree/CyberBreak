// GENERATED AUTOMATICALLY FROM 'Assets/InputControllers/TempBossControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TempBossControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TempBossControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TempBossControls"",
    ""maps"": [
        {
            ""name"": ""BossControl"",
            ""id"": ""5e1350fc-b0dd-4d6f-8c9d-48a689fcb5ca"",
            ""actions"": [
                {
                    ""name"": ""TestFire"",
                    ""type"": ""Button"",
                    ""id"": ""e44bdb56-c17a-49e0-a453-dd271a46da05"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""91214865-c6b2-4a43-ba41-b91cebb830d0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""BossTest"",
                    ""action"": ""TestFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""BossTest"",
            ""bindingGroup"": ""BossTest"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // BossControl
        m_BossControl = asset.FindActionMap("BossControl", throwIfNotFound: true);
        m_BossControl_TestFire = m_BossControl.FindAction("TestFire", throwIfNotFound: true);
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

    // BossControl
    private readonly InputActionMap m_BossControl;
    private IBossControlActions m_BossControlActionsCallbackInterface;
    private readonly InputAction m_BossControl_TestFire;
    public struct BossControlActions
    {
        private @TempBossControls m_Wrapper;
        public BossControlActions(@TempBossControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TestFire => m_Wrapper.m_BossControl_TestFire;
        public InputActionMap Get() { return m_Wrapper.m_BossControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BossControlActions set) { return set.Get(); }
        public void SetCallbacks(IBossControlActions instance)
        {
            if (m_Wrapper.m_BossControlActionsCallbackInterface != null)
            {
                @TestFire.started -= m_Wrapper.m_BossControlActionsCallbackInterface.OnTestFire;
                @TestFire.performed -= m_Wrapper.m_BossControlActionsCallbackInterface.OnTestFire;
                @TestFire.canceled -= m_Wrapper.m_BossControlActionsCallbackInterface.OnTestFire;
            }
            m_Wrapper.m_BossControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TestFire.started += instance.OnTestFire;
                @TestFire.performed += instance.OnTestFire;
                @TestFire.canceled += instance.OnTestFire;
            }
        }
    }
    public BossControlActions @BossControl => new BossControlActions(this);
    private int m_BossTestSchemeIndex = -1;
    public InputControlScheme BossTestScheme
    {
        get
        {
            if (m_BossTestSchemeIndex == -1) m_BossTestSchemeIndex = asset.FindControlSchemeIndex("BossTest");
            return asset.controlSchemes[m_BossTestSchemeIndex];
        }
    }
    public interface IBossControlActions
    {
        void OnTestFire(InputAction.CallbackContext context);
    }
}
