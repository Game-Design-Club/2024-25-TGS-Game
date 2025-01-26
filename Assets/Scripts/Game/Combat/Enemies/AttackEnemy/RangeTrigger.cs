using System;
using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class RangeTrigger : MonoBehaviour {
        [SerializeField] private AttackEnemyBase enemyBase;
        private void OnTriggerEnter2D(Collider2D other) {
            enemyBase.OnEntityEnterTrigger(other);
        }
    }
}