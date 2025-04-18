using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class Attack : EnemyState {
        public Attack(EnemyBase controller) : base(controller) { }
        
        private bool _shouldDie = false;

        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationConstants.TreeEnemy.Attack);
        }

        public override void Exit() {
            Controller().Animator.ResetTrigger(AnimationConstants.TreeEnemy.Reset);
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            Controller().Animator.SetTrigger(AnimationConstants.TreeEnemy.Reset);
            Controller().TransitionToState(new Retract(Controller()));
        }

        public override void Die() {
            _shouldDie = true;
        }

        public override void OnAnimationEnded() {
            if (_shouldDie) {
                Object.Destroy(Controller().gameObject);
            } else {
                Controller().TransitionToState(new Attack(Controller()));
            }
        }
    }
}