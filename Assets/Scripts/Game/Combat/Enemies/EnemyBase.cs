using UnityEngine;

namespace Game.Combat.Enemies {
    public abstract class EnemyBase : MonoBehaviour {
        [SerializeField] private int health = 100;
        [SerializeField] internal int sanityRestored = 100;
        [SerializeField] internal int sanityDamage = 10;
        
        internal CombatAreaManager CombatManager;
        
        internal Rigidbody2D Rigidbody;
        
        private void Awake() {
            TryGetComponent(out Rigidbody);
        }

        public void TakeDamage(int damage, Vector2 hitDirection, float knockbackForce) {
            health -= damage;
            
            ProcessHit(hitDirection, knockbackForce);
            
            if (health <= 0) {
                CombatManager.EnemyKilled(this);
                Die();
            }
        }

        internal void Die() {
            Destroy(gameObject);
        }

        public void HitChild() {
            CombatManager.ChildHit(this);
            Die();
        }
        
        internal abstract void ProcessHit(Vector2 hitDirection, float knockbackForce);
    }
}