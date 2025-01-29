using Game.Combat.Bear;
using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies {
    public class EnemyDamageDealer : MonoBehaviour {
        [SerializeField] private float hitForce = 10;
        [SerializeField] internal EnemyBase enemyBase;
        
        private bool _canDamage = true;

        private void OnEnable() {
            _canDamage = true;
            
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
        }

        private void OnGameEvent(GameEvent obj) {
            if (obj.GameEventType == GameEventType.ExploreEnter) {
                HandleHit();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!_canDamage) return;
            if (other.CompareTag(Constants.Tags.Child)) {
                enemyBase.HitChild();
                HandleHit();
            } else if (other.TryGetComponent(out BearController bear)) {
                Vector2 dif = (other.transform.position - transform.position).normalized;
                bear.OnHit(dif, hitForce);
                _canDamage = false;
                HandleHit();
            }
        }

        protected virtual void HandleHit() { }
    }
}