using System;
using Game.GameManagement;
using UnityEngine;

namespace Tools {
    public class GameEventDebugger : MonoBehaviour {
        [SerializeField] private string lastGameState;
        [SerializeField] private bool printState = true;
        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
        }
        
        private void OnGameEvent(GameEvent gameEvent) {
            if (printState) {
                Debug.Log($"GameEvent: {gameEvent.GameEventType} - {gameEvent.IsPaused}");
            }
            lastGameState = gameEvent.GameEventType.ToString();
        }
    }
}