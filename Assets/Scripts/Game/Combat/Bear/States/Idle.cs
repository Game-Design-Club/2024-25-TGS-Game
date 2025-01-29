
using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class Idle : BearState {
        public Idle(BearController controller) : base(controller) { }

        public override void Enter() {
            Controller.Animator.SetTrigger(Constants.Animator.Bear.Idle);
            Controller.Animator.ResetTrigger(Constants.Animator.Bear.Swipe);
            Controller.Animator.ResetTrigger(Constants.Animator.Bear.Growl);
            Controller.Animator.ResetTrigger(Constants.Animator.Bear.GrowlChargeup);
        }

        public override void OnSwipeInput() {
            Controller.TransitionToState(new Swipe(Controller));
        }
    }
}