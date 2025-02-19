using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Enemies.AttackEnemy {
    public class RangeTrigger : MonoBehaviour {
        [FormerlySerializedAs("enemyBase")] [SerializeField] private AttackEnemy enemy;
        private void OnTriggerEnter2D(Collider2D other) {
            enemy.OnEntityEnterTrigger(other);
        }
    }
}