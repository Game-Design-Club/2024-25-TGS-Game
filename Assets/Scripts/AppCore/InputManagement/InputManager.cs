using System;
using System.Collections.Generic;
using System.Numerics;
using Game.GameManagement;
using UnityEngine;
using Object = System.Object;

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
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
            
            _playerInputs.Disable();
            
            UnsubscribeFromUIInput();
            UnsubscribeFromChildInput();
            UnsubscribeFromBearInput();
        }

        private void OnGameEvent(GameEvent gameEvent) {
            switch (gameEvent.GameEventType) {
                case GameEventType.Bear:
                    _playerInputs.Bear.Enable();
                    _playerInputs.Child.Disable();
                    break;
                case GameEventType.Child:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Enable();
                    break;
                case GameEventType.CombatEnter:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Disable();
                    break;
                case GameEventType.ExploreEnter:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Enable();
                    break;
                case GameEventType.Cutscene:
                    _playerInputs.Bear.Disable();
                    _playerInputs.Child.Disable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}