using System.Collections;
using AppCore;
using AppCore.InputManagement;
using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class ChildController : MonoBehaviour {
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private AnimationCurve walkToSleepCurve;
        
        private Vector2 _movement;
        
        private bool _active = false;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        
        
        private void Awake() {
            TryGetComponent(out _rb);
            TryGetComponent(out _animator);
        }

        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
            App.Get<InputManager>().OnChildMovement += Move;
        }
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
            App.Get<InputManager>().OnChildMovement -= Move;
        }
        
        private void Move(Vector2 movement) {
            _movement = movement;
        }
        
        private void Update() {
            if (!_active) return;
            _rb.linearVelocity = _movement * walkSpeed;
        }
        
        private void OnGameEvent(GameEvent gameEvent) {
            switch (gameEvent.GameEventType) {
                case GameEventType.Explore:
                    _active = true;
                    _rb.bodyType = RigidbodyType2D.Dynamic;
                    break;
                case GameEventType.Combat:
                    _active = false;
                    _rb.bodyType = RigidbodyType2D.Static;
                    break;
                case GameEventType.CombatEnter:
                    _active = false;
                    _rb.linearVelocity = Vector2.zero;
                    break;
                case GameEventType.ExploreEnter:
                    _active = true;
                    _rb.bodyType = RigidbodyType2D.Dynamic;
                    _animator.SetBool(Constants.Animator.Child.Sleep, false);
                    _movement = Vector2.zero;
                    break;
                default:
                    _active = false;
                    _rb.bodyType = RigidbodyType2D.Static;
                    break;
            }
        }

        public void WalkToPoint(Vector3 position) {
            StartCoroutine(WalkToPointCoroutine(position));
        }

        private IEnumerator WalkToPointCoroutine(Vector3 position) {
            yield return this.MoveToPosition(_rb, position, walkToSleepCurve);
            _animator.SetBool(Constants.Animator.Child.Sleep, true);
        }
    }
}