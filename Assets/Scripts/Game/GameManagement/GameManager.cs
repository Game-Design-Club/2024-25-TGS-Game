using System;
using AppCore;
using AppCore.DataManagement;
using AppCore.DialogueManagement;
using AppCore.InputManagement;
using AppCore.SceneManagement;
using Tools;
using UnityEngine;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour {
        [SerializeField] public float transitionDuration = 1f;
        [SerializeField] public UIManager UIManager;
        
        public static float TransitionDuration => _instance.transitionDuration;
        
        public static Action<GameEvent> OnGameEvent;
        
        private static GameManager _instance;
        
        private static bool _isPaused;
        public static bool IsPaused {
            get => _isPaused;
            set {
                _isPaused = value;
                OnGameEvent?.Invoke(new GameEvent {
                    GameEventType = _gameEventType,
                    IsPaused = _isPaused
                });
            }
        }
        
        private static GameEventType _lastGameEventType;
        private static GameEventType _gameEventType;

        public static UIManager GetUIManager()
        {
            return _instance.UIManager;
        }
        public static GameEventType GameEventType {
            get => _gameEventType;
            private set {
                _lastGameEventType = _gameEventType;
                _gameEventType = value;
                OnGameEvent?.Invoke(new GameEvent {
                    GameEventType = _gameEventType,
                    IsPaused = IsPaused
                });
            }
        }

        private void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
                Debug.LogError("Two GameManagers in scene");
            }
        }

        private void OnDestroy() {
            if (_instance == this) {
                _instance = null;
                _isPaused = false;
            }
        }

        private void Start() {
            GameEventType = GameEventType.ExploreEnter;
            GameEventType = GameEventType.Explore;
        }

        private void OnEnable() {
            App.Get<InputManager>().OnUICancel += OnGamePaused;
            App.Get<DialogueManager>().OnDialogueStart += DialogueStart;
            App.Get<DialogueManager>().OnDialogueEnd += DialogueEnd;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnUICancel -= OnGamePaused;
            App.Get<DialogueManager>().OnDialogueStart -= DialogueStart;
            App.Get<DialogueManager>().OnDialogueEnd -= DialogueEnd;
        }

        private void OnApplicationFocus(bool hasFocus) {
            #if !UNITY_EDITOR
            if (!hasFocus) {
                OnGamePaused();
            }
            #endif
        }

        public static void OnGamePaused() {
            IsPaused = !IsPaused;
        }
        
        public static void StartTransitionToCombat() {
            GameEventType = GameEventType.CombatEnter;
        }
        
        public static void EndTransitionToCombat() {
            GameEventType = GameEventType.Combat;
        }

        public static void StartTransitionToExploration() {
            GameEventType = GameEventType.ExploreEnter;
        }

        public static void EndTransitionToExploration() {
            GameEventType = GameEventType.Explore;
        }

        private void DialogueStart() {
            GameEventType = GameEventType.Dialogue;
        }

        public static void DialogueEnd() {
            GameEventType = _lastGameEventType;
        }

        public static void OnBearDeath() {
            GameEventType = GameEventType.BearDeath;
        }

        public static void OnBearRevive() {
            GameEventType = GameEventType.CombatEnter;
        }

        public static void OnPlayerRespawn() {
            GameEventType = GameEventType.ExploreEnter;
        }

        public static void SetDialogue() {
            GameEventType = GameEventType.Dialogue;
        }
        
        public void SaveAndQuit() {
            App.Get<DataManager>().Save();
            App.Get<SceneLoader>().LoadScene(Scenes.MainMenu);
        }

        public static void StartCutscene() {
            GameEventType = GameEventType.Cutscene;
        }

        public static void EndCutscene() {
            GameEventType = _lastGameEventType;
        }
    }
    
    public struct GameEvent {
        public GameEventType GameEventType;
        public bool IsPaused;
    }
    
    public enum GameEventType {
        Cutscene,
        CombatEnter,
        ExploreEnter,
        Combat,
        Explore,
        Dialogue,
        BearDeath
    }
}
