using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class Move : AttackEnemyState {
        public Move(AttackEnemyBase controller) : base(controller) { }

        private Transform _targetTransform;
        public override void Enter() {
            _targetTransform = Controller.CombatManager.Child.transform;
        }

        public override void Exit() {
            Controller.Rigidbody2D.linearVelocity = Vector2.zero;
        }

        public override void Update() {
            Vector2 posDifference = -(Controller.transform.position - _targetTransform.position);
            if (posDifference.magnitude < Controller.attackRange) {
                Controller.TransitionToState(new Attack(Controller));
            } else {
                Controller.Rigidbody2D.linearVelocity = posDifference.normalized * Controller.walkSpeed;
            }
        }
    }
}