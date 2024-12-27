using System;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action OnUICancel;
        
        private void SubscribeToUIInput() {
            _playerInputs.UI.Cancel.performed += CallCancel;
        }
        
        private void UnsubscribeFromUIInput() {
            _playerInputs.UI.Cancel.performed -= CallCancel;
        }

        private void CallCancel(InputAction.CallbackContext obj) {
            OnUICancel?.Invoke();
        }
    }
}