using AppCore;
using AppCore.InputManagement;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearStateMachine : MonoBehaviour {
        private BearController _controller;
        
        private BearState _currentState;
        
        private Idle _idle = new Idle();

        private void OnEnable() {
            App.Get<InputManager>().OnBearMovement += OnMovement;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnBearMovement -= OnMovement;
        }

        private void Awake() {
            TryGetComponent(out _controller);
        }

        private void Start() {
            _idle.Controller = _controller;
            TransitionToState(_idle);
        }
        
        internal void TransitionToState(BearState newState) {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        private void Update() {
            _currentState?.Update();
        }
        
        private void OnMovement(Vector2 input) {
            _currentState?.OnMoveInput(input);
        }
    }
}