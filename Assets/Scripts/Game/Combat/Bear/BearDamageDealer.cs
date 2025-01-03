using Game.Combat.Enemies;
using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearDamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 50;
        [SerializeField] private float knockbackForce = 10;
        
        private bool _canDamage = true;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!_canDamage) return;
            if (!other.CompareTag(Constants.Tags.Enemy)) return;
            
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            
            other.GetComponent<EnemyBase>().TakeDamage(50, knockbackDirection, knockbackForce);
        }
    }
}