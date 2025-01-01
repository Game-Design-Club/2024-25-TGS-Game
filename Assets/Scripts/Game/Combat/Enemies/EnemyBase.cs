using UnityEngine;

namespace Game.Combat.Enemies {
    public abstract class EnemyBase : MonoBehaviour {
        [SerializeField] private int health = 100;
        [SerializeField] internal int sanityRestored = 100;
        [SerializeField] internal int sanityDamage = 10;
        
        internal CombatAreaManager CombatManager;

        public void TakeDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                Die();
            }
        }

        private void Die() {
            CombatManager.EnemyKilled(this);
            Destroy(gameObject);
        }
    }
}