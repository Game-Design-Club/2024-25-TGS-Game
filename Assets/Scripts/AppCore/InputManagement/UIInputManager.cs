using System;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action OnUICancel;
        public event Action OnDialogueContinue;
        
        private void SubscribeToUIInput() {
            _playerInputs.UI.Cancel.performed += CallCancel;
            _playerInputs.UI.Continue.performed += CallContinue;
        }
        
        private void UnsubscribeFromUIInput() {
            _playerInputs.UI.Cancel.performed -= CallCancel;
            _playerInputs.UI.Continue.performed -= CallContinue;
        }

        private void CallCancel(InputAction.CallbackContext obj) { OnUICancel?.Invoke(); }
        
        private void CallContinue(InputAction.CallbackContext obj) { OnDialogueContinue?.Invoke(); }
    }
}