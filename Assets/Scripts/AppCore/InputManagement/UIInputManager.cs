using System;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action OnUICancel;
        public event Action OnDialogueContinue;
        public event Action OnUIRestart;

        private void SubscribeToUIInput() {
            _playerInputs.UI.Cancel.performed += CallCancel;
            _playerInputs.UI.Continue.performed += CallContinue;
            _playerInputs.UI.Restart.performed += CallRestart;
        }
        
        private void UnsubscribeFromUIInput() {
            _playerInputs.UI.Cancel.performed -= CallCancel;
            _playerInputs.UI.Continue.performed -= CallContinue;
            _playerInputs.UI.Restart.performed -= CallRestart;
        }

        private void CallCancel(InputAction.CallbackContext obj) { OnUICancel?.Invoke(); }
        
        private void CallContinue(InputAction.CallbackContext obj) { OnDialogueContinue?.Invoke(); }
        
        private void CallRestart(InputAction.CallbackContext obj) { OnUIRestart?.Invoke(); }
    }
}