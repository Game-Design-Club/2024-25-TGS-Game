using UnityEngine;

namespace Game.Combat.Bear {
    public abstract class BearState {
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
        
        internal BearController Controller;

        internal virtual void OnMoveInput(Vector2 input) { }
    }
}