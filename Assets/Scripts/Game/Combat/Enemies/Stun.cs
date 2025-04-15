using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies {
    public class Stun : EnemyState {
        private Vector2 _hitDirection;
        private float _hitForce;
        private float _stunDuration;
        
        private float _progress = 0;
        
        private EnemyState _callbackState;

        public Stun(EnemyBase controller, Vector2 hitDirection, float hitForce, float stunDuration, EnemyState callbackState) : base(controller) {
            _hitDirection = hitDirection;
            _hitForce = hitForce;
            _callbackState = callbackState;
            _stunDuration = stunDuration;
        }
        
        public override void Enter() {
            if (_stunDuration > 0) {
                Controller().stunObject?.SetActive(true);
            }
        }

        public override void Exit() {
            if (_stunDuration > 0) {
                Controller().stunObject?.SetActive(false);
            }
        }

        public override void Update() {
            _progress += Time.deltaTime;
            
            if (_progress <= Controller().stunKnockbackCurve.Time()) {
                Controller().Rigidbody.linearVelocity = _hitDirection * (Controller().stunKnockbackCurve.Evaluate(_progress) * _hitForce);
            } else if (_progress <= Controller().stunKnockbackCurve.Time() + _stunDuration) {
                Controller().Rigidbody.linearVelocity = Vector2.zero;
            } else {
                Controller().TransitionToState(_callbackState);
            }
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            HandleHit(hitDirection, hitForce, damageType, _callbackState);
        }
    }
}