using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class Swipe : BearState {
        public Swipe(BearController controller) : base(controller) { }

        public override void Enter() {
            Controller.WalkSpeed = Controller.swipeWalkSpeed;
            Controller.Animator.SetTrigger(Constants.Animator.Bear.Swipe);
        }

        public override void Exit() {
            
        }

        public override void Update() {
            
        }
        
        public override void AnimationEnded() {
            Controller.StateMachine.TransitionToState(new Idle(Controller));
        }
    }
}