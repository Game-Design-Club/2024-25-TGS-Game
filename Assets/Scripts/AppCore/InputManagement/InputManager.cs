using System;
using Game.GameManagement;
using UnityEngine;

namespace AppCore.InputManagement {
    public partial class InputManager : AppModule {
        private PlayerInputs _playerInputs;
        
        private void Awake() {
            _playerInputs = new PlayerInputs();
        }
        
        private void OnEnable() {
            _playerInputs.Enable();
            _playerInputs.UI.Enable();
            
            GameManager.OnGameEvent += OnGameEvent;
            
            SubscribeToUIInput();
            SubscribeToChildInput();
            SubscribeToBearInput();
        }
        
        // things woo
        private void OnGameEvent(GameEvent gameEvent) {
            switch (gameEvent.GameEventType) {
                case GameEventType.Combat:
                    _playerInputs.Bear.Enable();
                    _playerInputs.Child.Disable();
                    _playerInputs.UI.Enable();
                    break;
                case GameEventType.Explore:
                case GameEventType.FinalEncounter:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Enable();
                    _playerInputs.UI.Enable();
                    break;
                case GameEventType.CombatEnter:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Disable();
                    _playerInputs.UI.Enable();
                    break;
                case GameEventType.ExploreEnter:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Disable();
                    _playerInputs.UI.Enable();
                    break;
                case GameEventType.Cutscene:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Disable();
                    _playerInputs.UI.Enable();
                    break;
                case GameEventType.Dialogue:
                case GameEventType.BearDeath:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Disable();
                    _playerInputs.UI.Enable();
                    break;
            }
        }
    }
}