using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action<Vector2> OnBearMovement;

        private void SubscribeToBearInput() {
            _playerInputs.Bear.Move.performed += CallBearMovement;
            _playerInputs.Bear.Move.canceled += CallBearMovement;
        }

        private void UnsubscribeFromBearInput() {
            _playerInputs.Bear.Move.performed -= CallBearMovement;
            _playerInputs.Bear.Move.canceled -= CallBearMovement;
        }

        private void CallBearMovement(InputAction.CallbackContext obj) {
            OnBearMovement?.Invoke(obj.ReadValue<Vector2>());
        }
    }
}