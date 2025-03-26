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
        [SerializeField] private string currentStateName = "CurrentState";
        [Header("References")]
        [SerializeField] private Transform rotateTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] internal BoxCollider2D boxCollider;
        [Header("Idle State")]
        [SerializeField] internal float walkSpeed = 5f;
        [Header("Jumping")]
        [SerializeField] internal float minJumpTime = 1f;
        [SerializeField] internal float maxJumpTime = 1.5f;
        [SerializeField] internal float jumpSpeed = 4f;
        [Header("Floating")]
        [SerializeField] internal float floatSpeed = 1f;
        [Header("Walk to Sleep")]
        [FormerlySerializedAs("walkToSleepCurve")]
        [SerializeField] internal AnimationCurve walkToPointCurve;
        [Header("SFX")]
        [SerializeField] internal SoundEffect walkSound;
        [Header("Misc")]
        [SerializeField] internal int childLayer;
        [SerializeField] internal int jumpableLayer;
        
        
        internal Rigidbody2D Rigidbody;
        internal Animator Animator;
        
        internal Vector2 LastInput;
        internal Vector2 LastDirection = Vector2.right;
        internal float LastRotation;
        internal float LastSpeed;
        
        private ChildState _currentState;
        public PlayerPointCollision NewPointCollision => new(transform.position);
        private Vector2? _forceDirection = null;

        private void Awake() {
            TryGetComponent(out Rigidbody);
            TryGetComponent(out Animator);
        }
        
        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
            App.Get<InputManager>().OnChildMovement += Move;
            App.Get<InputManager>().OnChildAttack += Attack;
            App.Get<InputManager>().OnChildJump += Jump;
            App.Get<InputManager>().OnChildJumpReleased += JumpReleased;
        }
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
            App.Get<InputManager>().OnChildMovement -= Move;
            App.Get<InputManager>().OnChildAttack -= Attack;
            App.Get<InputManager>().OnChildJump -= Jump;
            App.Get<InputManager>().OnChildJumpReleased -= JumpReleased;
        }

        private void Start() {
            Vector3 position;
            if (App.Get<DataManager>().firstLevelLoad) {
                position = LevelManager.GetCurrentLevel().spawnPoint.position;
            } else {
                position = App.Get<DataManager>().PlayerPosition;
            }
            transform.position = position;
            if (new PlayerPointCollision(transform.position).TouchingRiver) {
                TransitionToState(new Float(this));
            } else {
                TransitionToState(new Move(this));
            }
        }
        
        internal void TransitionToState(ChildState newState) {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
            currentStateName = _currentState.GetType().Name;
        }

        private void Update() {
            _currentState.Update();
            
            float? speed = _currentState.GetWalkSpeed();
            if (speed.HasValue) {
                Vector2 direction;
                if (_forceDirection.HasValue) {
                    direction = _forceDirection.Value;
                } else {
                    direction = _currentState.GetWalkDirection();
                }
                direction.Normalize();
                Rigidbody.linearVelocity = (float)speed * direction;
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
        
        private IEnumerator MoveUntilGrounded() {
            float xSize = boxCollider.size.x / 2;
            float ySize = boxCollider.size.y / 2;
            int i = 0;
            while (true) {
                Vector2 pos = Rigidbody.position;
                PlayerPointCollision topLeft = new PlayerPointCollision(pos + new Vector2(-xSize, ySize));
                PlayerPointCollision topRight = new PlayerPointCollision(pos + new Vector2(xSize, ySize));
                PlayerPointCollision bottomLeft = new PlayerPointCollision(pos + new Vector2(-xSize, -ySize));
                PlayerPointCollision bottomRight = new PlayerPointCollision(pos + new Vector2(xSize, -ySize));
            
                if (topLeft.TouchingLand && topRight.TouchingLand && bottomLeft.TouchingLand && bottomRight.TouchingLand) {
                    break;
                }
            
                _forceDirection = Vector2.zero;
                if (topLeft.TouchingLand) {
                    _forceDirection += new Vector2(-1, 1);
                }
                if (topRight.TouchingLand) {
                    _forceDirection += new Vector2(1, 1);
                }
                if (bottomLeft.TouchingLand) {
                    _forceDirection += new Vector2(-1, -1);
                }
                if (bottomRight.TouchingLand) {
                    _forceDirection += new Vector2(1, -1);
                }

                if (_forceDirection == Vector2.zero) {
                    Debug.LogWarning("No force direction on child");
                    break;
                }

                yield return new WaitForFixedUpdate();
                i++;
                if (i > 500) {
                    Debug.LogError("Jumping for way too long");
                    break;
                }
            }
            TransitionToState(new Move(this));
            _forceDirection = null;
        }

        private void Attack() { _currentState.OnAttackInput(); }

        private void Jump() { _currentState.OnJumpInput(); }

        private void JumpReleased() { _currentState.OnJumpInputReleased(); }

        private void AttackAnimationEnded() { _currentState.OnAttackAnimationOver(); }

        private void OnGameEvent(GameEvent gameEvent) { _currentState?.OnGameEvent(gameEvent); }

        public void Sleep(Vector3 position) { _currentState.Sleep(position); }

        public bool CanInteract() { return _currentState.CanInteract(); }

        internal void StartMoveUntilGrounded() { StartCoroutine(MoveUntilGrounded()); }

        public void LandPlayer() {
            Vector2 pos = Rigidbody.position;
            float xSize = boxCollider.size.x / 2;
            float ySize = boxCollider.size.y / 2;
            
            PlayerPointCollision topLeft = new PlayerPointCollision(pos + new Vector2(-xSize, ySize));
            PlayerPointCollision topRight = new PlayerPointCollision(pos + new Vector2(xSize, ySize));
            PlayerPointCollision bottomLeft = new PlayerPointCollision(pos + new Vector2(-xSize, -ySize));
            PlayerPointCollision bottomRight = new PlayerPointCollision(pos + new Vector2(xSize, -ySize));
            
            if (DoAll(point => point.TouchingGround)) {
                TransitionToState(new Move(this));
                return;
            }
            
            if (DoAny(point => point.TouchingLog) || DoAny(point => point.TouchingRock)) {
                StartMoveUntilGrounded();
                return;
            }
            
            if (DoAll(point => point.TouchingRiver)) {
                TransitionToState(new Float(this));
                return;
            }
            
            StartMoveUntilGrounded();
            
            bool DoAll(Func<PlayerPointCollision, bool> predicate) {
                return predicate(topLeft) && predicate(topRight) && predicate(bottomLeft) && predicate(bottomRight);
            }

            bool DoAny(Func<PlayerPointCollision, bool> predicate) {
                return predicate(topLeft) || predicate(topRight) || predicate(bottomLeft) || predicate(bottomRight);
            }

        }
    }
}