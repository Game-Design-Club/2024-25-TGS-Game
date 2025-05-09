using AppCore;
using AppCore.DataManagement;
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
            Controller.Animator.SetTrigger(AnimationParameters.Bear.Stun);
            Controller.stunObject.SetActive(true);
        }
        
        public override void Exit() {
            Controller.stunObject.SetActive(false);
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
            if (_progress >= Controller.stunKnockbackCurve.Time()) {
                _completedKnockback = true;
            }
        }
        
        public override void OnAnimationEnded(int id) {
            if (id != AnimationParameters.BearIDs.Stun) return;
            if (App.Get<InputManager>().GetBearSwipe && App.Get<DataManager>().GetFlag(BoolFlags.HasGrowl)) {
                Controller.TransitionToState(new GrowlChargeup(Controller));
            } else {
                Controller.TransitionToState(new Idle(Controller));
            }
        }
    }
}