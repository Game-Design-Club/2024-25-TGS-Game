using Game.Combat.Enemies;
using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearDamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 50;
        
        private bool _canDamage = true;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!_canDamage) return;
            if (!other.CompareTag(Constants.Tags.Enemy)) return;
            
            other.GetComponent<EnemyBase>().TakeDamage(50);
        }
    }
}