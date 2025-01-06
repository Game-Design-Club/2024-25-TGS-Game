using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action<Vector2> OnBearMovement;
        public event Action OnBearSwipe;
        private void SubscribeToBearInput() {
            _playerInputs.Bear.Move.performed += CallBearMovement;
            _playerInputs.Bear.Move.canceled += CallBearMovement;
            _playerInputs.Bear.Swipe.performed += CallBearSwipe;
        }

        private void UnsubscribeFromBearInput() {
            _playerInputs.Bear.Move.performed -= CallBearMovement;
            _playerInputs.Bear.Move.canceled -= CallBearMovement;
            _playerInputs.Bear.Swipe.performed -= CallBearSwipe;
        }

        private void CallBearMovement(InputAction.CallbackContext obj) { OnBearMovement?.Invoke(obj.ReadValue<Vector2>()); }
        
        private void CallBearSwipe(InputAction.CallbackContext obj) { OnBearSwipe?.Invoke(); }
    }
}