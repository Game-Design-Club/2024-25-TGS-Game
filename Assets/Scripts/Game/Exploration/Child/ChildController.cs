using AppCore;
using AppCore.InputManagement;
using Game.Combat;
using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class ChildController : MonoBehaviour {
        [SerializeField] private float walkSpeed = 5f;
        
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
                case GameEventType.Child:
                    _active = true;
                    _rb.bodyType = RigidbodyType2D.Dynamic;
                    break;
                case GameEventType.Bear:
                    _active = false;
                    _rb.bodyType = RigidbodyType2D.Static;
                    break;
                case GameEventType.CombatEnter:
                    _active = false;
                    _rb.bodyType = RigidbodyType2D.Static;
                    _animator.SetBool(Constants.Animator.Child.Sleep, true);
                    break;
                case GameEventType.ExploreEnter:
                    _active = true;
                    _rb.bodyType = RigidbodyType2D.Dynamic;
                    _animator.SetBool(Constants.Animator.Child.Sleep, false);
                    break;
                default:
                    _active = false;
                    _rb.bodyType = RigidbodyType2D.Static;
                    break;
            }
        }
    }
}