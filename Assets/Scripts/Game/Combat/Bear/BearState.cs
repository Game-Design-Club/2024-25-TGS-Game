using UnityEngine;

namespace Game.Combat.Bear {
    public abstract class BearState {
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
        
        internal BearController Controller;

        public virtual void OnMoveInput(Vector2 input) { }
        public virtual void OnSwipeInput() { }
        public virtual void AnimationEnded() { }
    }
}