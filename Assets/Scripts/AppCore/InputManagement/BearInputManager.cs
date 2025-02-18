using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action<Vector2> OnBearMovement;
        public event Action OnBearSwipe;
        public event Action OnBearSwipeReleased;
        public event Action OnBearPounce;
        public event Action OnBearPounceReleased;
        
        public bool GetBearSwipe => _playerInputs.Bear.Swipe.IsPressed();

        private void SubscribeToBearInput() {
            _playerInputs.Bear.Move.performed += CallBearMovement;
            _playerInputs.Bear.Move.canceled += CallBearMovement;
            _playerInputs.Bear.Swipe.started += CallBearSwipe;
            _playerInputs.Bear.Swipe.canceled += CallBearSwipeReleased;
            _playerInputs.Bear.Pounce.performed += CallBearPounce;
            _playerInputs.Bear.Pounce.canceled += CallBearPounceReleased;
        }

        private void UnsubscribeFromBearInput() {
            _playerInputs.Bear.Move.performed -= CallBearMovement;
            _playerInputs.Bear.Move.canceled -= CallBearMovement;
            _playerInputs.Bear.Swipe.started -= CallBearSwipe;
            _playerInputs.Bear.Swipe.canceled -= CallBearSwipeReleased;
            _playerInputs.Bear.Pounce.performed -= CallBearPounce;
            _playerInputs.Bear.Pounce.canceled -= CallBearPounceReleased;
        }

        private void CallBearMovement(InputAction.CallbackContext obj) { OnBearMovement?.Invoke(obj.ReadValue<Vector2>()); }
        private void CallBearSwipe(InputAction.CallbackContext obj) { OnBearSwipe?.Invoke(); }
        private void CallBearSwipeReleased(InputAction.CallbackContext obj) { OnBearSwipeReleased?.Invoke(); }
        private void CallBearPounce(InputAction.CallbackContext obj) { OnBearPounce?.Invoke(); }
        private void CallBearPounceReleased(InputAction.CallbackContext obj) { OnBearPounceReleased?.Invoke(); }
    }
}