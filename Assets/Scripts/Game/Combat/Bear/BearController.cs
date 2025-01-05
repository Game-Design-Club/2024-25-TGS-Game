using AppCore;
using AppCore.InputManagement;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Bear {
    public class BearController : MonoBehaviour {
        [SerializeField] internal float idleWalkSpeed = 5f;
        [SerializeField] internal float swipeWalkSpeed = 2f;
        
        internal Vector2 LastInput;
        internal float LastRotation;
        
        internal Animator Animator;
        internal Rigidbody2D Rigidbody2D;
        
        private BearState _currentState;
        
        private void OnEnable() {
            App.Get<InputManager>().OnBearSwipe += OnSwipe;
            App.Get<InputManager>().OnBearMovement += OnMovement;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnBearSwipe -= OnSwipe;
            App.Get<InputManager>().OnBearMovement += OnMovement;
        }
        
        private void Awake() {
            TryGetComponent(out Animator);
            TryGetComponent(out Rigidbody2D);
        }

        private void Start() {
            TransitionToState(new Idle(this));
        }
        
        internal void TransitionToState(BearState newState) {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        private void Update() {
            _currentState.Update();
            Rigidbody2D.linearVelocity = _currentState.GetWalkSpeed() * _currentState.GetWalkDirection();
           
            float? rotation = _currentState.GetRotation();
            if (rotation.HasValue) {
                transform.rotation = Quaternion.Euler(0, 0, (float)rotation);
                LastRotation = (float)rotation;
            }        
        }
        
        // Exposed to Animation Events
        private void OnSwipe() {
            _currentState.OnSwipeInput();
        }
        
        private void AnimationEnded() {
            _currentState.OnAnimationEnded();
        }
        
        private void OnMovement(Vector2 direction) {
            LastInput = direction;
            _currentState.OnMovementInput(direction);
        }
    }
}