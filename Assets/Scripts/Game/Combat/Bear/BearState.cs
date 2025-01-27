using UnityEngine;

namespace Game.Combat.Bear {
    public abstract class BearState {
        internal readonly BearController Controller;
        
        protected BearState(BearController controller) {
            Controller = controller;
        }
        
        // State
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }

        // Input
        public virtual void OnMovementInput(Vector2 direction) { }
        public virtual void OnSwipeInput() { }
        public virtual void OnSwipeInputReleased() { }
        public virtual void OnAnimationEnded(int id) { }
        
        // Functions
        public virtual float? GetWalkSpeed() {
            return Controller.idleWalkSpeed;
        }
        public virtual Vector2 GetWalkDirection() {
            return Controller.LastInput;
        }
        public virtual float? GetRotation() {
            return DefaultRotation(Controller.LastInput);
        }
        public virtual void OnHit(Vector2 hitDirection, float hitForce) {
            Controller.TransitionToState(new Stun(Controller, hitDirection, hitForce));
        }

        private float? DefaultRotation(Vector2 input) {
            if (input.x > 0 && input.y > 0) return 45;
            if (input.x > 0 && input.y < 0) return 315;
            if (input.x < 0 && input.y > 0) return 135;
            if (input.x < 0 && input.y < 0) return 225;
            if (input.x > 0) return 0;
            if (input.x < 0) return 180;
            if (input.y > 0) return 90;
            if (input.y < 0) return 270;
            return null;
        }
    }
}