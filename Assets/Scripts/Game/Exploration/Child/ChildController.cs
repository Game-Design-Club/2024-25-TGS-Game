using System;
using AppCore;
using AppCore.InputManagement;
using Game.GameManagement;
using UnityEngine;

namespace Game.Child {
    public class ChildController : MonoBehaviour {
        [SerializeField] private AnimationCurve startWalkCurve;
        [SerializeField] private AnimationCurve stopWalkCurve;
        [SerializeField] private float walkSpeed = 5f;
        
        private Vector2 _movement;
        
        private Rigidbody2D _rb;
        
        private bool _active = false;
        
        private void Awake() {
            TryGetComponent(out _rb);
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
                default:
                    _active = false;
                    _rb.bodyType = RigidbodyType2D.Static;
                    break;
            }
        }
    }
}