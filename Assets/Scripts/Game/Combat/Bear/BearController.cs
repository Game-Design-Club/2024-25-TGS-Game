using AppCore;
using AppCore.InputManagement;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Transform rotateTransform;
        [Header("Idle State")]
        [SerializeField] internal float idleWalkSpeed = 5f;
        [Header("Swipe State")]
        [SerializeField] internal float swipeWalkSpeed = 2f;
        
        internal Vector2 LastInput;
        internal float LastRotation;
        internal float LastSpeed;
        
        internal Animator Animator;
        internal Rigidbody2D Rigidbody2D;
        
        private BearState _currentState;
        
        private void OnEnable() {
            App.Get<InputManager>().OnBearSwipe += OnSwipe;
            App.Get<InputManager>().OnBearSwipeReleased += OnSwipeReleased;
            App.Get<InputManager>().OnBearMovement += OnMovement;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnBearSwipe -= OnSwipe;
            App.Get<InputManager>().OnBearSwipeReleased -= OnSwipeReleased;
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
            
            float? speed = _currentState.GetWalkSpeed();
            if (speed.HasValue) {
                Rigidbody2D.linearVelocity = (float)speed * _currentState.GetWalkDirection();
                LastSpeed = (float)speed;
            }
           
            float? rotation = _currentState.GetRotation();
            if (rotation.HasValue) {
                LastRotation = (float)rotation;
                
                // Debug.Log(rotation);
                if (rotation > 90 && rotation < 270) {
                    rotateTransform.localScale = new Vector3(-1, 1, 1);
                    rotation += 180;
                } else {
                    rotateTransform.localScale = new Vector3(1, 1, 1);
                }
                rotateTransform.rotation = Quaternion.Euler(0, 0, (float)rotation);
            }
        }
        
        // Exposed to Animation Events
        private void OnSwipe() {
            _currentState.OnSwipeInput();
        }
        
        private void OnSwipeReleased() {
            _currentState.OnSwipeInputReleased();
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