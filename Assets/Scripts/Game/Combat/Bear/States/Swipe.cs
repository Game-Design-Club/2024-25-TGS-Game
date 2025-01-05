using System;
using Tools;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Combat.Bear {
    public class Swipe : BearState {
        public Swipe(BearController controller) : base(controller) { }

        private float _startRotation;
        
        public override void Enter() {
            Controller.Animator.SetTrigger(Constants.Animator.Bear.Swipe);
            _startRotation = Controller.LastRotation;
        }

        public override float GetWalkSpeed() {
            return Controller.swipeWalkSpeed;
        }
        
        public override float? GetRotation() {
            return _startRotation;
        }

        public override void OnAnimationEnded() {
            Controller.TransitionToState(new Idle(Controller));
        }
    }
}