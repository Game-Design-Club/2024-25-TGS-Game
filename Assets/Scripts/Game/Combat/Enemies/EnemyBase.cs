using UnityEngine;

namespace Game.Combat.Enemies {
    public abstract class EnemyBase : MonoBehaviour {
        [SerializeField] private int health = 100;
        [SerializeField] internal int sanityRestored = 100;
        [SerializeField] internal int sanityDamage = 10;
        
        internal CombatAreaManager CombatManager;
        
        public void TakeDamage(int damage, Vector2 hitDirection, float knockbackForce) {
            health -= damage;
            
            ProcessHit(hitDirection, knockbackForce);
            
            if (health <= 0) {
                CombatManager.EnemyKilled(this);
                HandleDeath();
            }
        }

        protected abstract void HandleDeath();

        public void HitChild() {
            CombatManager.ChildHit(this);
            HandleDeath();
        }
        
        internal abstract void ProcessHit(Vector2 hitDirection, float knockbackForce);
    }
}