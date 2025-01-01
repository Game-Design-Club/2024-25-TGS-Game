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
    }
}