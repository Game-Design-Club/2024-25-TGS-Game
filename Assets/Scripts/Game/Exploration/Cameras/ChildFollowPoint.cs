using System;
using Game.GameManagement;
using UnityEngine;

namespace Game.Exploration.Cameras {
    public class ChildFollowPoint : MonoBehaviour {
        [SerializeField] public bool lockX;
        [SerializeField] public bool lockY;
        
        Transform _targetTransform;
        
        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
        }
        
        private void OnGameEvent(GameEvent obj) {
            if (obj.GameEventType == GameEventType.ExploreEnter) {
                _targetTransform = LevelManager.GetCurrentLevel().child.transform;
            }
        }
        
        private void Start() {
            _targetTransform = LevelManager.GetCurrentLevel().child.transform;
        }

        private void Update() {
            if (_targetTransform == null) return;
            
            Vector3 targetPosition = _targetTransform.position;
            Vector3 currentPosition = transform.position;

            if (lockX) {
                targetPosition.x = currentPosition.x;
            }

            if (lockY) {
                targetPosition.y = currentPosition.y;
            }

            transform.position = targetPosition;
        }
    }
}