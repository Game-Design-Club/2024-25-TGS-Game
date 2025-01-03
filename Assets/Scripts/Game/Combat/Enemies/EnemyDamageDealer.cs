using Tools;
using UnityEngine;

namespace Game.Combat.Enemies {
    public class EnemyDamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 50;
        
        private bool _canDamage = true;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!_canDamage) return;
            if (!other.CompareTag(Constants.Tags.Child)) return;
            
            GetComponent<EnemyBase>().HitChild();
        }
    }
}