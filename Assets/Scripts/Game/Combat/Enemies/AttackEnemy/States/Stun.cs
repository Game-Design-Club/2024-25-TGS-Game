using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class Stun : AttackEnemyState {
        private Vector2 _hitDirection;
        private float _hitForce;
        
        private float _progress = 0;

        public Stun(AttackEnemyBase controller, Vector2 hitDirection, float hitForce) : base(controller) {
            _hitDirection = hitDirection;
            _hitForce = hitForce;
        }
        public override void Enter() {
        }

        public override void Exit() {
        }

        public override void Update() {
            _progress += Time.deltaTime;
            Controller.Rigidbody.linearVelocity = _hitDirection * (Controller.stunKnockbackCurve.Evaluate(_progress) * _hitForce);
            
            if (_progress >= Controller.stunKnockbackCurve.keys[Controller.stunKnockbackCurve.length - 1].time) {
                Controller.TransitionToState(new Move(Controller));
            }
        }
    }
}