using System;
using System.Collections.Generic;
using Game.Combat.Enemies;
using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearDamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 50;
        [SerializeField] private float knockbackForce = 10;
        [SerializeField] private float directionWeight = 0.6f;
        [SerializeField] private bool movementBased = true;
        
        private bool _canDamage = true;
        
        private HashSet<EnemyBase> _enemiesHit = new();

        private void OnEnable() {
            _enemiesHit.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!_canDamage) return;
            if (!other.gameObject.TryGetComponent(out EnemyBase enemyBase) || !_enemiesHit.Add(enemyBase)) return; // Add returns false if already in set

            Vector2 dif= (other.transform.position - transform.position).normalized;
            int flip = transform.lossyScale.x > 0 ? 1 : -1;

            Vector2 knockbackDirection;
            if (movementBased) {
                knockbackDirection = ((Vector2)transform.right + dif * directionWeight) * flip;
            } else {
                knockbackDirection = dif;
            }
            
            enemyBase.TakeDamage(50, knockbackDirection.normalized, knockbackForce);
        }
    }
}