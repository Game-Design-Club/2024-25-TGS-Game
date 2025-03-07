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

        public void Sleep(Vector3 position) {
            Controller.TransitionToState(new Sleep(Controller, position));
        }
    }
}