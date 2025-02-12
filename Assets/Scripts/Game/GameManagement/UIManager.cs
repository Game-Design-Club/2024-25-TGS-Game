using System;
using System.Collections;
using AppCore;
using AppCore.InputManagement;
using Tools;
using UnityEngine;

namespace Game.GameManagement {
    public class UIManager : MonoBehaviour {
        [SerializeField] private Animator pauseAnimator;
        [SerializeField] private Animator gameOverAnimator;
        
        private bool _isGameOver = false;
        private bool _canRestart = false;
        
        public static event Action OnRestartGame;

        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
            App.Get<InputManager>().OnUIRestart += OnUIContinue;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
            App.Get<InputManager>().OnUIRestart -= OnUIContinue;
        }

        private void OnGameEvent(GameEvent gameEvent) {
            if (gameEvent.GameEventType == GameEventType.BearDeath) {
                HandleGameOver();
            }
            else {
                HandlePause(gameEvent);
            }
        }
        private void HandlePause(GameEvent gameEvent) {
            Time.timeScale = gameEvent.IsPaused ? 0 : 1;
            pauseAnimator.SetBool(Constants.Animator.GameUI.IsPaused, gameEvent.IsPaused);
        }

        private void HandleGameOver() {
            gameOverAnimator.SetBool(Constants.Animator.GameUI.IsGameOver, true);
            _isGameOver = true;
            StartCoroutine(WaitToRestartGame());
        }

        private IEnumerator WaitToRestartGame() {
            yield return new WaitForSeconds(GameManager.TransitionDuration);
            _canRestart = true;
        }

        private void OnUIContinue() {
            if (_isGameOver && _canRestart) {
                _isGameOver = false;
                _canRestart = false;

                gameOverAnimator.SetBool(Constants.Animator.GameUI.IsGameOver, false);
                
                OnRestartGame?.Invoke();
            }
        }
    }
}