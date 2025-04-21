using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    internal class Attack : EnemyState {
        public Attack(AttackEnemy controller) : base(controller) { }
        
        private bool _died = false;
        
        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationParameters.AttackEnemy.Attack);
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            HandleHit(hitDirection, hitForce, damageType, new Move(Controller()));
        }

        public override void OnAnimationEnded() {
            if (_died) {
                Object.Destroy(Controller().gameObject);
            } else {
                Controller().TransitionToState(new Move(Controller()));
            }
        }

        public override void Die() {
            _died = true;
        }
    }
}