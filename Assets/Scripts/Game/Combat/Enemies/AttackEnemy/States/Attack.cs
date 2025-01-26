using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class Attack : AttackEnemyState {
        public Attack(AttackEnemyBase controller) : base(controller) { }
        
        private bool died = false;
        
        public override void Enter() {
            Controller.Animator.SetTrigger(Constants.Animator.Enemy.Attack);
        }

        public override void OnAnimationEnded() {
            if (died) {
                Object.Destroy(Controller.gameObject);
            } else {
                Controller.TransitionToState(new Move(Controller));
            }
        }

        public override void Die() {
            died = true;
        }
    }
}