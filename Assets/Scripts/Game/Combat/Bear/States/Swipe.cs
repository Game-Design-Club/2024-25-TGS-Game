using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class Swipe : BearState {
        public Swipe(BearController controller) : base(controller) { }

        private float _startRotation;
        
        private bool _swipeInputReleased = false;
        
        public override void Enter() {
            Controller.Animator.SetTrigger(Constants.Animator.Bear.Swipe);
            _startRotation = Controller.LastRotation;
        }

        public override float? GetWalkSpeed() {
            return Controller.swipeWalkSpeed;
        }
        
        public override float? GetRotation() {
            return _startRotation;
        }
        
        public override void OnSwipeInputReleased() {
            _swipeInputReleased = true;
        }

        public override void OnAnimationEnded() {
            if (!_swipeInputReleased) {
                Controller.TransitionToState(new GrowlChargeup(Controller));
            } else {
                Controller.TransitionToState(new Idle(Controller));
            }
        }
    }
}