using Game.GameManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tools {
    public class UnityOnGameEvent : MonoBehaviour {
        [SerializeField] private GameEventSubscription[] subscriptions;
        
        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEventReceived;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEventReceived;
        }

        private void OnGameEventReceived(GameEvent gameEvent) {
            foreach (var subscription in subscriptions) {
                if (subscription.gameEvent == gameEvent.GameEventType) {
                    subscription.action.Invoke();
                }
            }
        }
    }
    
    [System.Serializable]
    public struct GameEventSubscription {
        public GameEventType gameEvent;
        public UnityEvent action;
    }
}
