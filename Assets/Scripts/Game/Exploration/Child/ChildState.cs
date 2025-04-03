using Game.GameManagement;
using UnityEngine;

namespace Game.Exploration.Child {
    public abstract class ChildState {
        internal readonly ChildController Controller;
        
        protected ChildState(ChildController controller) {
            Controller = controller;
        }
        
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void OnMovementInput(Vector2 direction) { }

        public virtual void OnAttackInput(){}
        public virtual void OnJumpInput() {}
        public virtual void OnJumpInputReleased() { }
        public virtual void OnAttackAnimationOver(){}

        public virtual float? GetWalkSpeed() {
            return Controller.walkSpeed;
        }
        
        public virtual Vector2 GetWalkDirection() {
            return Controller.LastInput;
        }
        
        public virtual float? GetRotation() {
            return DefaultRotation(Controller.LastInput);
        }
        
        public virtual void OnGameEvent(GameEvent gameEvent) { }
        
        private float? DefaultRotation(Vector2 input) {
            if (input.x > 0 && input.y > 0) return 45; // up right
            if (input.x > 0 && input.y < 0) return 315; // down right
            if (input.x < 0 && input.y > 0) return 135; // up left
            if (input.x < 0 && input.y < 0) return 225; // down left
            if (input.x > 0) return 0; // right
            if (input.x < 0) return 180; // left
            if (input.y > 0) return 90; // up
            if (input.y < 0) return 270; // down
            return null;
        }

        public void Sleep(Vector3 position) {
            Controller.TransitionToState(new Sleep(Controller, position));
        }

        public virtual bool CanInteract() { return true; }
    }
}