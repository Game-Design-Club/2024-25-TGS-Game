using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action<Vector2> OnChildMovement;
        public event Action OnChildInteract;

        private void SubscribeToChildInput() {
            _playerInputs.Child.Move.performed += CallChildMovement;
            _playerInputs.Child.Move.canceled += CallChildMovement;
            _playerInputs.Child.Interact.performed += CallChildInteract;
        }
        
        private void UnsubscribeFromChildInput() {
            _playerInputs.Child.Move.performed -= CallChildMovement;
            _playerInputs.Child.Move.canceled -= CallChildMovement;
            _playerInputs.Child.Interact.performed -= CallChildInteract;
        }
        
        private void CallChildMovement(InputAction.CallbackContext ctx) { OnChildMovement?.Invoke(ctx.ReadValue<Vector2>()); }
        
        private void CallChildInteract(InputAction.CallbackContext ctx) { OnChildInteract?.Invoke(); }
    }
}