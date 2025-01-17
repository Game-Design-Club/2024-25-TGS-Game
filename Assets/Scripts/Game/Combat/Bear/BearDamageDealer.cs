using Game.Combat.Enemies;
using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearDamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 50;
        [SerializeField] private float knockbackForce = 10;
        [Range(0f,1f)]
        [SerializeField] private float directionWeight = 0.6f;
        
        private bool _canDamage = true;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!_canDamage) return;
            if (!other.CompareTag(Constants.Tags.Enemy)) return;

            Vector2 dif= (other.transform.position - transform.position).normalized * directionWeight;
            
            Vector2 knockbackDirection = ((Vector2)transform.right + dif) * (transform.lossyScale.x > 0 ? 1 : -1);
            
            other.GetComponent<EnemyBase>().TakeDamage(50, knockbackDirection.normalized, knockbackForce);
        }
    }
}