using UnityEngine;

namespace Game.Combat.Bear {
    public class Idle : BearState {
        private Vector2 _velocity;
        
        public override void Enter() {
            
        }

        public override void Exit() {
            
        }

        public override void Update() {
            Controller.Rigidbody2D.linearVelocity = _velocity * Controller.idleWalkSpeed;
        }
        
        internal override void OnMoveInput(Vector2 input) {
            _velocity = input;
        }
    }
}