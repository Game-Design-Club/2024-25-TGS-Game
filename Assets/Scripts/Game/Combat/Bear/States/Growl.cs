using Tools;

namespace Game.Combat.Bear {
    public class Growl : BearState {
        public Growl(BearController controller) : base(controller) { }
        
        private bool _swipeBuffer = false;
        
        public override void Enter() {
            Controller.Animator.SetTrigger(Constants.Animator.Bear.Growl);
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
            if (id != Constants.Animator.BearIDs.Growl) return;
            if (_swipeBuffer) {
                Controller.TransitionToState(new Swipe(Controller));
            } else {
                Controller.TransitionToState(new Idle(Controller));
            }
        }
    }
}