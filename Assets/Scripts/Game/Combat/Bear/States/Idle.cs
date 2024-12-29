using UnityEngine;

namespace Game.Combat.Bear {
    public class Idle : BearState {
        public override void Enter() {
            Controller.WalkSpeed = Controller.idleWalkSpeed;
        }

        public override void Exit() {
        }

        public override void Update() {
        }
        
        public override void OnSwipeInput() {
            Controller.StateMachine.TransitionToState(Controller.StateMachine.Swipe);
        }
    }
}