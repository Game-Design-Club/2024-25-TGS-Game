using UnityEngine;

namespace Game.Combat.Enemies {
    public abstract class EnemyBase : MonoBehaviour {
        [SerializeField] private int health = 100;
        
        public void TakeDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                Die();
            }
        }

        private void Die() {
            Destroy(gameObject);
        }
    }
}