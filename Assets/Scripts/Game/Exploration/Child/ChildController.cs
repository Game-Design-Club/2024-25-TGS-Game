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
        [SerializeField] internal SpriteRenderer spriteRenderer;
        [FormerlySerializedAs("boxCollider")] [SerializeField] internal BoxCollider2D mainBoxCollider;
        [SerializeField] internal BoxCollider2D combatBoxCollider;
        [SerializeField] internal BoxCollider2D chaseBoxCollider;
        [SerializeField] internal Animator _spriteAnimator;
        [Header("Idle State")]
        [SerializeField] internal float walkSpeed = 5f;
        [Header("Attack")]
        [SerializeField] internal ParticleSystem swishParticles;
        [SerializeField] internal Transform swishMatchTransform;
        [Header("Jumping")]
        [SerializeField] internal AnimationCurve jumpSpeedCurve;
        [SerializeField] internal float jumpSpeed = 4f;
        [SerializeField] internal ParticleSystem jumpParticles;
        [SerializeField] internal ParticleSystem landGroundParticles;
        [SerializeField] internal ParticleSystem landRiverParticles;
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
        public PointCollision NewPointCollision => new(transform.position);
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
            if (new PointCollision(transform.position).TouchingRiver) {
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
                _spriteAnimator.SetFloat(AnimationParameters.ChildSprites.MoveX, LastDirection.x);
                _spriteAnimator.SetFloat(AnimationParameters.ChildSprites.MoveY, LastDirection.y);
            }
            
            App.Get<DataManager>().UpdatePlayerPosition(transform.position);
        }
        
        private void Move(Vector2 direction) {
            LastInput = direction;
            if (direction != Vector2.zero) LastDirection = direction;
            _currentState.OnMovementInput(direction);
        }
        
        private IEnumerator MoveUntilGrounded() {
            yield return PointCollisionHelper.MoveToInArea(
                mainBoxCollider,
                Rigidbody,
                dir => _forceDirection = dir,
                point => point.TouchingLand);
            TransitionToState(new Move(this));
            _forceDirection = null;
        }
        

        private void Attack() { _currentState.OnAttackInput(); }

        private void Jump() { _currentState.OnJumpInput(); }

        private void JumpReleased() { _currentState.OnJumpInputReleased(); }

        private void AttackAnimationEnded() { _currentState.OnAttackAnimationOver(); }

        private void OnGameEvent(GameEvent gameEvent) {
            _currentState?.OnGameEvent(gameEvent);
            if (gameEvent.GameEventType == GameEventType.Combat) {
                combatBoxCollider.enabled = true;
            } else if (gameEvent.GameEventType == GameEventType.Explore) {
                combatBoxCollider.enabled = false;
            }
        }

        public void Sleep(Vector3 position) { _currentState.Sleep(position); }

        public bool CanInteract() { return _currentState.CanInteract(); }

        internal void StartMoveUntilGrounded() { StartCoroutine(MoveUntilGrounded()); }

        public void LandPlayer() {
            Vector2 pos = Rigidbody.position;
            float xSize = mainBoxCollider.size.x / 2;
            float ySize = mainBoxCollider.size.y / 2;
            
            PointCollision topLeft = new PointCollision(pos + new Vector2(-xSize, ySize));
            PointCollision topRight = new PointCollision(pos + new Vector2(xSize, ySize));
            PointCollision bottomLeft = new PointCollision(pos + new Vector2(-xSize, -ySize));
            PointCollision bottomRight = new PointCollision(pos + new Vector2(xSize, -ySize));
            
            if (DoAll(point => point.TouchingGround)) {
                TransitionToState(new Move(this));
                landGroundParticles.Play();
                return;
            }
            
            if (DoAny(point => point.TouchingLog) || DoAny(point => point.TouchingRock)) {
                StartMoveUntilGrounded();
                landGroundParticles.Play();
                return;
            }
            
            if (DoAll(point => point.TouchingRiver)) {
                TransitionToState(new Float(this));
                landRiverParticles.Play();
                return;
            }
            
            StartMoveUntilGrounded();
            
            bool DoAll(Func<PointCollision, bool> predicate) {
                return predicate(topLeft) && predicate(topRight) && predicate(bottomLeft) && predicate(bottomRight);
            }

            bool DoAny(Func<PointCollision, bool> predicate) {
                return predicate(topLeft) || predicate(topRight) || predicate(bottomLeft) || predicate(bottomRight);
            }

        }

        public void ExploreDie() {
            _currentState.ExploreDie();
        }

        public void UnDie() {
            if (_currentState is Die) {
                TransitionToState(new Move(this));
            }
        }
    }
}