using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action<Vector2> OnChildMovement;
        public event Action OnChildInteract;
        public event Action OnChildAttack;

        private void SubscribeToChildInput() {
            _playerInputs.Child.Move.performed += CallChildMovement;
            _playerInputs.Child.Move.canceled += CallChildMovement;
            _playerInputs.Child.Interact.performed += CallChildInteract;
            _playerInputs.Child.Attack.performed += CallChildAttack;
        }
        
        private void UnsubscribeFromChildInput() {
            _playerInputs.Child.Move.performed -= CallChildMovement;
            _playerInputs.Child.Move.canceled -= CallChildMovement;
            _playerInputs.Child.Interact.performed -= CallChildInteract;
            _playerInputs.Child.Attack.performed -= CallChildAttack;
        }
        
        private void CallChildMovement(InputAction.CallbackContext ctx) { OnChildMovement?.Invoke(ctx.ReadValue<Vector2>()); }
        
        private void CallChildInteract(InputAction.CallbackContext ctx) { OnChildInteract?.Invoke(); }
        private void CallChildAttack(InputAction.CallbackContext ctx) { OnChildAttack?.Invoke(); }
    }
}