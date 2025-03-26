using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public partial class InputManager {
        public event Action<Vector2> OnChildMovement;
        public event Action OnChildInteract;
        public event Action OnChildAttack;
        public event Action OnChildJump;
        public event Action OnChildJumpReleased;
        public bool GetChildJump => _playerInputs.Child.Jump.IsPressed();

        private void SubscribeToChildInput() {
            _playerInputs.Child.Move.performed += ctx => OnChildMovement?.Invoke(ctx.ReadValue<Vector2>());
            _playerInputs.Child.Move.canceled += ctx => OnChildMovement?.Invoke(ctx.ReadValue<Vector2>());
            _playerInputs.Child.Interact.performed += _ => OnChildInteract?.Invoke();
            _playerInputs.Child.Attack.performed += _ => OnChildAttack?.Invoke();
            _playerInputs.Child.Jump.performed += _ => OnChildJump?.Invoke();
            _playerInputs.Child.Jump.canceled += _ => OnChildJumpReleased?.Invoke();
        }
    }
}