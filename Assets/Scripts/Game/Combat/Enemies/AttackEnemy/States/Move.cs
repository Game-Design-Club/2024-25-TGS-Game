using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    internal class Move : EnemyState {
        public Move(EnemyBase controller) : base(controller) { }

        private Transform _targetTransform;
        public override void Enter() {
            Controller().Animator.SetTrigger(Constants.Animator.AttackEnemy.Idle);
            _targetTransform = Controller().CombatManager.Child.transform;
        }
        
        public override void OnHit(Vector2 hitDirection, float hitForce) {
            HandleHit(hitDirection, hitForce, new Move(Controller()));
        }

        public override void Exit() {
            Controller().Rigidbody.linearVelocity = Vector2.zero;
        }

        public override void Update() {
            Vector2 posDifference = -(Controller().transform.position - _targetTransform.position);
            Controller().Rigidbody.linearVelocity = posDifference.normalized * Controller<AttackEnemy>().moveSpeed;
            Controller().Rigidbody.rotation = Mathf.Atan2(posDifference.y, posDifference.x) * Mathf.Rad2Deg;
        }
    }
}