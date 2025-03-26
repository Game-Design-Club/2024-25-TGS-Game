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
            _playerInputs.Bear.Move.performed += ctx => OnBearMovement?.Invoke(ctx.ReadValue<Vector2>());
            _playerInputs.Bear.Move.canceled += ctx => OnBearMovement?.Invoke(ctx.ReadValue<Vector2>());
            _playerInputs.Bear.Swipe.performed += _ => OnBearSwipe?.Invoke();
            _playerInputs.Bear.Swipe.canceled += _ => OnBearSwipeReleased?.Invoke();
            _playerInputs.Bear.Pounce.performed += _ => OnBearPounce?.Invoke();
            _playerInputs.Bear.Pounce.canceled += _ => OnBearPounceReleased?.Invoke();
        }
    }
}