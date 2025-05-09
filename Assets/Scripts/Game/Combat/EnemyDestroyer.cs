using System;
using Game.Combat.Enemies;
using UnityEngine;

namespace Game.Combat {
    public class EnemyDestroyer : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent(out EnemyBase enemy)) {
                Destroy(enemy.gameObject);
            }
        }
    }
}