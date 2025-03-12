using Tools;

namespace Game.Combat.Bear {
    public class Growl : BearState {
        public Growl(BearController controller) : base(controller) { }
        
        private bool _swipeBuffer = false;
        
        public override void Enter() {
            Controller.Animator.SetTrigger(AnimationConstants.Bear.Growl);
        }

        public override float? GetWalkSpeed() {
            return 0;
        }

        public override void OnSwipeInput() {
            _swipeBuffer = true;
        }
        
        public override void OnSwipeInputReleased() {
            _swipeBuffer = false;
        }

        public override void OnAnimationEnded(int id) {
            if (id != AnimationConstants.BearIDs.Growl) return;
            if (_swipeBuffer) {
                Controller.TransitionToState(new Swipe(Controller));
            } else {
                Controller.TransitionToState(new Idle(Controller));
            }
        }
    }
}