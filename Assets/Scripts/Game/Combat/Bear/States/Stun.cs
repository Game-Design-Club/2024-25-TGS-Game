using AppCore;
using AppCore.InputManagement;
using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class Stun : BearState {
        private Vector2 _hitDirection;
        private float _hitForce;
        
        private float _progress = 0;
        
        private bool _completedKnockback = false;

        public Stun(BearController controller, Vector2 hitDirection, float hitForce) : base(controller) {
            _hitDirection = hitDirection;
            _hitForce = hitForce;
        }
        public override void Enter() {
            Controller.Animator.SetTrigger(Constants.Animator.Bear.Stun);
        }

        public override float? GetWalkSpeed() {
            return Controller.stunKnockbackCurve.Evaluate(_progress) * _hitForce;
        }
        
        public override Vector2 GetWalkDirection() {
            return _hitDirection;
        }

        public override void Update() {
            if (_completedKnockback) return;
            _progress += Time.deltaTime;
            if (_progress >= Controller.stunKnockbackCurve.keys[Controller.stunKnockbackCurve.length - 1].time) {
                _completedKnockback = true;
            }
        }
        
        public override void OnAnimationEnded(int id) {
            if (id != Constants.Animator.BearIDs.Stun) return;
            if (App.Get<InputManager>().GetBearSwipe) {
                Controller.TransitionToState(new GrowlChargeup(Controller));
            } else {
                Controller.TransitionToState(new Idle(Controller));
            }
        }
    }
}