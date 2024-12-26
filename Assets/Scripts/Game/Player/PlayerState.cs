using UnityEngine;

namespace Player {
    public interface IPlayerState {
        public void OnExit();
        public void OnEnter();
        public void OnUpdate();
    }
}