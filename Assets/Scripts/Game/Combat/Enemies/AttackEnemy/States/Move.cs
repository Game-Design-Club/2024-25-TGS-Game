using Game.Combat.Bear;
using Tools;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    internal class Move : EnemyState {
        public Move(EnemyBase controller) : base(controller) { }

        private Transform _targetTransform;
        
        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationConstants.AttackEnemy.Idle);
            _targetTransform = Controller().CombatManager.Child.transform;
        }
        
        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            HandleHit(hitDirection, hitForce, new Move(Controller()));
        }

        public override void Exit() {
            Controller().Rigidbody.linearVelocity = Vector2.zero;
        }

        public override void Update() {
            Vector2 posDifference = -(Controller().transform.position - _targetTransform.position);
            Controller().Rigidbody.linearVelocity = posDifference.normalized * Controller<AttackEnemy>().moveSpeed;
            Controller<AttackEnemy>().rotationPivot.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(posDifference.y, posDifference.x) * Mathf.Rad2Deg);
            Controller<AttackEnemy>().spriteRenderer.flipX = Controller<AttackEnemy>().StartingFlipX ? posDifference.x > 0 : posDifference.x < 0;
        }
    }
}