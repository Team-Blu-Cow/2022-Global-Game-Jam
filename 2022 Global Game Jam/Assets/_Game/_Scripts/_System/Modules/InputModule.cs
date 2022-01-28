using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;

namespace blu
{
    public class InputModule : Module
    {
        private MasterInput _input;
        public MasterInput Input { get => _input; }

        private InputDevice m_LastUsedDevice;
        public InputDevice LastUsedDevice { get => m_LastUsedDevice; }

        public delegate void LastDeviceChangedDelegate();

        public event LastDeviceChangedDelegate LastDeviceChanged;

        public override void Initialize()
        {
            // If functions run twice after scene switch take a look at this: https://answers.unity.com/questions/1767382/removing-event-from-new-input-system-not-working.html
            Debug.Log("[App]: Initializing input module");
            SetUpControllers();
        }

        private void SetUpControllers()
        {
            _input = new MasterInput();
            _input.Enable();
        }

        private void OnEnable()
        {
            InputSystem.onEvent += OnInputDeviceChange;
        }

        private void OnDisable()
        {
            InputSystem.onEvent -= OnInputDeviceChange;
        }

        private void OnInputDeviceChange(InputEventPtr eventPtr, InputDevice device)
        {
            if (m_LastUsedDevice == device)
                return;

            m_LastUsedDevice = device;
            LastDeviceChanged?.Invoke();
        }
    }
}