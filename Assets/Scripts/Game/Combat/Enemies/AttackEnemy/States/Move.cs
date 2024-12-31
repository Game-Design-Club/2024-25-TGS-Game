using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class Move : AttackEnemyState {
        public Move(AttackEnemyBase @base) : base(@base) { }

        private Transform _t;
        public override void Enter() {
            _t = Base.CombatManager.Child.transform;
        }

        public override void Exit() {
            Base.Rigidbody2D.linearVelocity = Vector2.zero;
        }

        public override void Update() {
            Vector2 posDifference = -(Base.transform.position - _t.position);
            if (posDifference.magnitude < Base.attackRange) {
                Base.TransitionToState(new Attack(Base));
            } else {
                Base.Rigidbody2D.linearVelocity = posDifference.normalized * Base.walkSpeed;
            }
        }
    }
}