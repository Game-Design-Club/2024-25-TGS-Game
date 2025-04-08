using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class Idle : BearState {
        public Idle(BearController controller) : base(controller) { }
        public Idle(BearController controller, float cooldown) : base(controller) {
            _cooldown = cooldown;
        }
        
        private float _cooldown = 0;
        
        public override void Enter() {
            Controller.Animator.SetTrigger(AnimationConstants.Bear.Idle);
            Controller.Animator.ResetTrigger(AnimationConstants.Bear.Swipe);
            Controller.Animator.ResetTrigger(AnimationConstants.Bear.Growl);
            Controller.Animator.ResetTrigger(AnimationConstants.Bear.GrowlChargeup);
        }
        
        public override void Update() {
            if (_cooldown > 0) {
                _cooldown -= Time.deltaTime;
            }
        }

        public override void OnSwipeInput() {
            if (_cooldown > 0) return;
            Controller.TransitionToState(new Swipe(Controller));
        }

        public override void OnPounceInput() {
            if (_cooldown > 0) return;
            Controller.TransitionToState(new Pounce(Controller));
        }
    }
}