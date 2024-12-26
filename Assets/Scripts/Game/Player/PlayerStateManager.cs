using UnityEngine;

namespace Player {
    public class PlayerStateManager : MonoBehaviour {
        private IPlayerState _currentState;
        
        public void SetState(IPlayerState newState) {
            if (_currentState != null) {
                _currentState.OnExit();
            }
            _currentState = newState;
            _currentState.OnEnter();
        }
        
        private void Update() {
            if (_currentState != null) {
                _currentState.OnUpdate();
            }
        }
    }
}