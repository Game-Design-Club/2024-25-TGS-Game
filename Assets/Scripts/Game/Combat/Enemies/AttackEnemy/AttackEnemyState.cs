using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public abstract class AttackEnemyState {
        protected AttackEnemyBase Controller;

        protected AttackEnemyState(AttackEnemyBase controller) {
            Controller = controller;
        }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();

        public virtual void OnHit(Vector2 hitDirection, float hitForce) {
            Stun stun = new Stun(Controller, hitDirection, hitForce);
            Controller.TransitionToState(stun);
        }
    }
}