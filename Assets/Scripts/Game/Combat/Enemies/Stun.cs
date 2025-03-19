using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies {
    public class Stun : EnemyState {
        private Vector2 _hitDirection;
        private float _hitForce;
        
        private float _progress = 0;
        
        private EnemyState _callbackState;

        public Stun(EnemyBase controller, Vector2 hitDirection, float hitForce, EnemyState callbackState) : base(controller) {
            _hitDirection = hitDirection;
            _hitForce = hitForce;
            _callbackState = callbackState;
        }
        public override void Enter() {
        }

        public override void Exit() {
        }

        public override void Update() {
            _progress += Time.deltaTime;
            Controller().Rigidbody.linearVelocity = _hitDirection * (Controller().stunKnockbackCurve.Evaluate(_progress) * _hitForce);
            
            if (_progress >= Controller().stunKnockbackCurve.Time()) {
                Controller().TransitionToState(_callbackState);
            }
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            HandleHit(hitDirection, hitForce, _callbackState);
        }
    }
}