using AppCore;
using AppCore.InputManagement;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearStateMachine : MonoBehaviour {
        private BearController _controller;
        
        private BearState _currentState;
        
        internal readonly Idle Idle = new Idle();
        internal readonly Swipe Swipe = new Swipe();

        private void OnEnable() {
            App.Get<InputManager>().OnBearSwipe += OnSwipe;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnBearSwipe -= OnSwipe;
        }

        private void Awake() {
            TryGetComponent(out _controller);
            _controller.StateMachine = this;
        }

        private void Start() {
            Idle.Controller = _controller;
            Swipe.Controller = _controller;
            TransitionToState(Idle);
        }
        
        internal void TransitionToState(BearState newState) {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        private void Update() {
            _currentState?.Update();
        }
        
        private void OnSwipe() {
            _currentState?.OnSwipeInput();
        }
        
        // Exposed to Animation Events
        private void AnimationEnded() {
            _currentState?.AnimationEnded();
        }
    }
}