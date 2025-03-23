using System;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action OnUICancel;
        public event Action OnDialogueContinue;
        public event Action OnUIRestart;

        private void SubscribeToUIInput() {
            _playerInputs.UI.Cancel.performed += _ => OnUICancel?.Invoke();
            _playerInputs.UI.Continue.performed += _ => OnDialogueContinue?.Invoke();
            _playerInputs.UI.Restart.performed += _ => OnUIRestart?.Invoke();
        }
    }
}