using System;
using Game.Combat.Bear;
using Game.Exploration.Child;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Enemies {
    public class EnemyDamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 50;
        [SerializeField] private float hitForce = 10;
        
        [SerializeField] private EnemyBase enemyBase;
        
        private bool _canDamage = true;

        private void OnEnable() {
            _canDamage = true;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!_canDamage) return;
            if (other.CompareTag(Constants.Tags.Child)) {
                enemyBase.HitChild();
                Destroy(enemyBase.gameObject);
            } else if (other.TryGetComponent(out BearController bear)) {
                Vector2 dif = (other.transform.position - transform.position).normalized;
                bear.OnHit(dif, hitForce);
                _canDamage = false;
            }
        }
    }
}