using AppCore;
using AppCore.AudioManagement;
using AppCore.InputManagement;
using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class Swipe : BearState {
        public Swipe(BearController controller) : base(controller) { }

        private float _startRotation;
        
        public override void Enter() {
            Controller.Animator.SetTrigger(AnimationConstants.Bear.Swipe);
            _startRotation = Controller.LastRotation;
            Controller.swipeSoundEffect.Play();
        }

        public override float? GetWalkSpeed() {
            return Controller.swipeWalkSpeed;
        }
        
        public override float? GetRotation() {
            return _startRotation;
        }
        
        public override void OnAnimationEnded(int id) {
            if (id != AnimationConstants.BearIDs.Swipe) return;
            if (!App.Get<InputManager>().GetBearSwipe) {
                Controller.TransitionToState(new GrowlChargeup(Controller));
            } else {
                Controller.TransitionToState(new Idle(Controller));
            }
        }
    }
}