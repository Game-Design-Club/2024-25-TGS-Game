using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class Attack : EnemyState {
        public Attack(EnemyBase controller) : base(controller) { }
        
        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationParameters.TreeEnemy.Attack);
        }

        public override void Exit() {
            Controller().Animator.ResetTrigger(AnimationParameters.TreeEnemy.Reset);
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            Controller().Animator.SetTrigger(AnimationParameters.TreeEnemy.Reset);
            Controller().TransitionToState(new Retract(Controller()));
        }

        public override void Die() {
            
        }

        public override void Update() {
            Controller().CombatManager.Sanity -= Time.deltaTime * Controller<TreeEnemy>().damageSpeed;
        }

        public override void OnAnimationEnded() {
            Controller().TransitionToState(new Attack(Controller()));
        }
    }
}