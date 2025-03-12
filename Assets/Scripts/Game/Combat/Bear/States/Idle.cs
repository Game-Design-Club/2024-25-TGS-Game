using Tools;

namespace Game.Combat.Bear {
    public class Idle : BearState {
        public Idle(BearController controller) : base(controller) { }

        public override void Enter() {
            Controller.Animator.SetTrigger(AnimationConstants.Bear.Idle);
            Controller.Animator.ResetTrigger(AnimationConstants.Bear.Swipe);
            Controller.Animator.ResetTrigger(AnimationConstants.Bear.Growl);
            Controller.Animator.ResetTrigger(AnimationConstants.Bear.GrowlChargeup);
        }

        public override void OnSwipeInput() {
            Controller.TransitionToState(new Swipe(Controller));
        }

        public override void OnPounceInput() {
            Controller.TransitionToState(new Pounce(Controller));
        }
    }
}