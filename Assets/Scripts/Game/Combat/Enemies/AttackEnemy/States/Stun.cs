using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class Stun : AttackEnemyState {
        internal Vector2 HitDirection;
        internal float HitForce;
        
        private float _progress = 0;

        public Stun(AttackEnemyBase controller) : base(controller) { }
        public override void Enter() {
        }

        public override void Exit() {
        }

        public override void Update() {
            _progress += Time.deltaTime;
            Controller.Rigidbody.linearVelocity = HitDirection * (Controller.stunKnockbackCurve.Evaluate(_progress) * HitForce);
            
            if (_progress >= Controller.stunKnockbackCurve.keys[Controller.stunKnockbackCurve.length - 1].time) {
                Controller.TransitionToState(new Move(Controller));
            }
        }
    }
}