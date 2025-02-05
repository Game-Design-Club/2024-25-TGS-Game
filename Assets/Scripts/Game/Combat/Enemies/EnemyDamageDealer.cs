using Game.Combat.Bear;
using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies {
    public class EnemyDamageDealer : MonoBehaviour {
        [SerializeField] private float hitForce = 10;
        [SerializeField] internal EnemyBase enemyBase;
        [SerializeField] private bool hitChild = true;
        [SerializeField] private bool hitBear = true;
        
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
            if (hitChild && other.CompareTag(Constants.Tags.Child)) {
                enemyBase.HitChild();
                HandleHit();
            }
            if (hitBear && _canDamage && other.TryGetComponent(out BearController bear)) {
                Vector2 dif = GetDirection(other);
                bear.OnHit(dif, hitForce);
                _canDamage = false;
                HandleHit();
            }
        }
        
        protected virtual Vector2 GetDirection(Collider2D other) {
            return (other.transform.position - transform.position).normalized;
        }

        protected virtual void HandleHit() { }
    }
}