using AppCore;
using AppCore.InputManagement;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearController : MonoBehaviour {
        [SerializeField] private string currentStateName = "State";
        [Header("References")]
        [SerializeField] private Transform rotateTransform;
        [Header("Idle State")]
        [SerializeField] internal float idleWalkSpeed = 5f;
        [Header("Swipe State")]
        [SerializeField] internal float swipeWalkSpeed = 2f;
        [Header("Stun State")]
        [SerializeField] internal AnimationCurve stunKnockbackCurve;
        
        
        internal Vector2 LastInput;
        internal float LastRotation;
        internal float LastSpeed;
        
        internal Animator Animator;
        internal Rigidbody2D Rigidbody;
        
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
            TryGetComponent(out Rigidbody);
        }

        private void Start() {
            TransitionToState(new Idle(this));
        }
        
        internal void TransitionToState(BearState newState) {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
            currentStateName = _currentState.GetType().Name;
        }

        private void Update() {
            _currentState.Update();
            
            float? speed = _currentState.GetWalkSpeed();
            if (speed.HasValue) {
                Rigidbody.linearVelocity = (float)speed * _currentState.GetWalkDirection();
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

        internal void OnHit(Vector2 hitDirection, float hitForce) {
            _currentState.OnHit(hitDirection, hitForce);
        }
    }
}