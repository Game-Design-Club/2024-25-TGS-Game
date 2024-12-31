using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public abstract class AttackEnemyState {
        protected AttackEnemyBase Base;

        protected AttackEnemyState(AttackEnemyBase @base) {
            Base = @base;
        }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}