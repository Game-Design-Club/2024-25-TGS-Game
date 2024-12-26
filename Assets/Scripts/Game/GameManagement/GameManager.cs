using System;
using AppCore;
using AppCore.InputManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour {
        // Child/Bear, and Paused/Unpaused
        // Controls InputMapping, and transitions between states
        public static Action<GameEvent> OnGameEvent;
        
        private bool _isPaused;
        private bool IsPaused {
            get => _isPaused;
            set {
                _isPaused = value;
                OnGameEvent?.Invoke(new GameEvent {
                    GameEventType = GameEventType.Cutscene,
                    IsPaused = _isPaused
                });
            }
        }
        
        private GameEventType _gameEventType;
        private GameEventType GameEventType {
            get => _gameEventType;
            set {
                _gameEventType = value;
                OnGameEvent?.Invoke(new GameEvent {
                    GameEventType = _gameEventType,
                    IsPaused = IsPaused
                });
            }
        }

        private void OnEnable() {
            App.Get<InputManager>().PlayerInputs.UI.Cancel.performed += OnGamePaused;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().PlayerInputs.UI.Cancel.performed -= OnGamePaused;
        }
        
        private void OnGamePaused(InputAction.CallbackContext ctx) {
            IsPaused = !IsPaused;
        }
    }
    
    public struct GameEvent {
        public GameEventType GameEventType;
        public bool IsPaused;
    }
    
    public enum GameEventType {
        Cutscene,
        Bear,
        Child
    }
}
