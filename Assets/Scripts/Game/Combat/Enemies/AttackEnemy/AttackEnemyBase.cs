using System;
using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class AttackEnemyBase : EnemyBase {
        [SerializeField] internal float attackRange = 1f;
        [SerializeField] internal float walkSpeed = 5f;

        
        private AttackEnemyState _currentState;
        
        private void Start() {
            TransitionToState(new Move(this));
        }

        internal void TransitionToState(AttackEnemyState newState) {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
        
        private void Update() {
            _currentState?.Update();
        }
    }
}