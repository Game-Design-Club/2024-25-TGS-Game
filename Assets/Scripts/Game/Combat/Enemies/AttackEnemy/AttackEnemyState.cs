using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public abstract class AttackEnemyState {
        protected AttackEnemyBase Controller;

        protected AttackEnemyState(AttackEnemyBase controller) {
            Controller = controller;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }
        public virtual void Update() { }

        public virtual void OnHit(Vector2 hitDirection, float hitForce) {
            Controller.TransitionToState(new Stun(Controller, hitDirection, hitForce));
        }
        
        public virtual void OnAnimationEnded() { }

        public virtual void Die() {
            Object.Destroy(Controller.gameObject);
        }
    }
}