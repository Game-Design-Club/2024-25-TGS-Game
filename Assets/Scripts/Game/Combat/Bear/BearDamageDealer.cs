using System.Collections.Generic;
using Game.Combat.Enemies;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearDamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 50;
        [SerializeField] private float knockbackForce = 10;
        [SerializeField] private bool movementBased = true;
        [SerializeField] private float directionWeight = 0.6f;
        
        private readonly HashSet<IBearHittable> _enemiesHit = new();

        private void OnEnable() {
            _enemiesHit.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.TryGetComponent(out IBearHittable enemyBase) && _enemiesHit.Add(enemyBase)) { // Add returns false if already in set
                AttackEnemy(enemyBase);
            }
        }

        private void AttackEnemy(IBearHittable other) {
            Vector2 dif= (other.GameObject.transform.position - transform.position).normalized;
            int flip = transform.lossyScale.x > 0 ? 1 : -1;

            Vector2 knockbackDirection;
            if (movementBased) {
                knockbackDirection = ((Vector2)transform.right + dif * directionWeight) * flip;
            } else {
                knockbackDirection = dif;
            }
            
            other.OnHit(damage, knockbackDirection.normalized, knockbackForce);
        }
    }
}