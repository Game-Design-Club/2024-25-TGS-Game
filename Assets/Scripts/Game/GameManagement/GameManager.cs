using System;
using AppCore;
using AppCore.InputManagement;
using UnityEngine;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour {
        // Child/Bear, and Paused/Unpaused
        // Controls InputMapping, and transitions between states
        
        public static Action<GameEvent> OnGameEvent;
        
        private static bool _isPaused;
        private static bool IsPaused {
            get => _isPaused;
            set {
                _isPaused = value;
                OnGameEvent?.Invoke(new GameEvent {
                    GameEventType = _gameEventType,
                    IsPaused = _isPaused
                });
            }
        }
        
        private static GameEventType _gameEventType;
        private static GameEventType GameEventType {
            get => _gameEventType;
            set {
                _gameEventType = value;
                OnGameEvent?.Invoke(new GameEvent {
                    GameEventType = _gameEventType,
                    IsPaused = IsPaused
                });
            }
        }

        private void Start() {
            GameEventType = GameEventType.Child;
        }

        private void OnEnable() {
            App.Get<InputManager>().OnUICancel += OnGamePaused;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnUICancel -= OnGamePaused;
        }
        
        private void OnGamePaused() {
            IsPaused = !IsPaused;
        }
        
        public static void StartTransitionToCombat() {
            GameEventType = GameEventType.Cutscene;
        }
        
        public static void EndTransitionToCombat() {
            GameEventType = GameEventType.Bear;
        }
        
        public static void StartTransitionToChild() {
            GameEventType = GameEventType.Cutscene;
        }
        
        public static void EndTransitionToChild() {
            GameEventType = GameEventType.Child;
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
