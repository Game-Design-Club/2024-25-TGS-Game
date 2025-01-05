using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class Stun : AttackEnemyState {
        internal Vector2 HitDirection;
        internal float HitForce;

        public Stun(AttackEnemyBase controller) : base(controller) { }
        public override void Enter() {
            Controller.Rigidbody.AddForce(HitDirection * HitForce, ForceMode2D.Impulse);
        }

        public override void Exit() {
        }

        public override void Update() {
            if (Controller.Rigidbody.linearVelocity.magnitude < 0.1f) {
                Controller.TransitionToState(new Move(Controller));
            }
        }
    }
}