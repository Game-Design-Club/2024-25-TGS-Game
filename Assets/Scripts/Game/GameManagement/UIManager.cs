using System;
using Tools;
using UnityEngine;

namespace Game.GameManagement {
    public class UIManager : MonoBehaviour {
        private Animator _animator;
        private void Awake() {
            TryGetComponent(out _animator);
        }
        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
        }

        private void OnGameEvent(GameEvent gameEvent) {
            Time.timeScale = gameEvent.IsPaused ? 0 : 1;
            _animator.SetBool(Constants.Animator.GameUI.IsPaused, gameEvent.IsPaused);
        }
    }
}