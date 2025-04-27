using System.Collections;
using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class GrowlChargeup : BearState {
        public GrowlChargeup(BearController controller) : base(controller) { }
        
        private bool _ready = false;
        
        public override float? GetWalkSpeed() {
            return Controller.swipeWalkSpeed;
        }

        public override void Enter() {
            Controller.Animator.SetTrigger(AnimationParameters.Bear.GrowlChargeup);
        }

        public override void OnAnimationEnded(int id) {
            if (id != AnimationParameters.BearIDs.GrowlChargeup) return;
            _ready = true;
        }

        public override void OnSwipeInputReleased() {
            if (_ready) {
                Controller.TransitionToState(new Growl(Controller));
            } else {
                Controller.TransitionToState(new Idle(Controller));
            }
        }
    }
}