using System;
using System.Collections;
using AppCore;
using AppCore.AudioManagement;
using AppCore.DataManagement;
using AppCore.InputManagement;
using Game.GameManagement;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Exploration.Child {
    public class ChildController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Transform rotateTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Header("Idle State")]
        [SerializeField] internal float walkSpeed = 5f;
        [FormerlySerializedAs("walkToSleepCurve")]
        [Header("Walk to Sleep")]
        [SerializeField] internal AnimationCurve walkToPointCurve;
        [Header("SFX")]
        [SerializeField] internal SoundEffect walkSound;
        
        internal Rigidbody2D Rigidbody;
        internal Animator Animator;
        
        internal Vector2 LastInput;
        internal Vector2 LastDirection = Vector2.right;
        internal float LastRotation;
        internal float LastSpeed;
        
        private ChildState _currentState;
        
        private void Awake() {
            TryGetComponent(out Rigidbody);
            TryGetComponent(out Animator);
        }
        
        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
            App.Get<InputManager>().OnChildMovement += Move;
            App.Get<InputManager>().OnChildAttack += Attack;
        }
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
            App.Get<InputManager>().OnChildMovement -= Move;
            App.Get<InputManager>().OnChildAttack -= Attack;
        }

        private void Start() {
            Vector3? position = App.Get<DataManager>().PlayerPosition;
            transform.position = (Vector3)position;
            TransitionToState(new Move(this));
        }
        
        internal void TransitionToState(ChildState newState) {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        private void Update() {
            _currentState.Update();
            
            float? speed = _currentState.GetWalkSpeed();
            if (speed.HasValue) {
                Rigidbody.linearVelocity = (float)speed * _currentState.GetWalkDirection();
                LastSpeed = (float)speed;
            }
            
            float? rotation = _currentState.GetRotation();
            if (rotateTransform != null && rotation.HasValue) {
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
            
            App.Get<DataManager>().UpdatePlayerPosition(transform.position);
            
            spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        }
        
        private void Move(Vector2 direction) {
            LastInput = direction;
            if (direction != Vector2.zero) LastDirection = direction;
            _currentState.OnMovementInput(direction);
        }

        private void Attack()
        {
            _currentState.OnAttackInput();
        }

        private void AttackAnimationEnded()
        {
            _currentState.OnAttackAnimationOver();
        }

        private void OnGameEvent(GameEvent gameEvent) {
            _currentState?.OnGameEvent(gameEvent);
        }
        
        public void Sleep(Vector3 position) {
            _currentState.Sleep(position);
        }
    }
}