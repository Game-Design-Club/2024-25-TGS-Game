using System;
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
        [SerializeField] internal float sanityDamage = 10;
        [SerializeField] private bool killChildOnAttack = true;
        
        private CombatAreaManager _combatManager;
        
        private bool _canDamage = true;

        private void OnEnable() {
            _canDamage = true;
            
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
        }

        private void Start() {
            _combatManager = enemyBase.CombatManager;
        }

        private void OnGameEvent(GameEvent obj) {
            if (obj.GameEventType == GameEventType.CombatEnter) {
                HandleCombatRestart();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (hitChild && other.CompareTag(Constants.Tags.Child)) {
                if (enemyBase != null && killChildOnAttack) {
                    enemyBase.HitChild();
                }
                HandleHit();
                _combatManager.ChildHit(this);
            }
            if (hitBear && _canDamage && other.TryGetComponent(out BearEnemyHitbox bearEnemyHitbox)) {
                Vector2 dif = GetDirection(other);
                bearEnemyHitbox.bearController.OnHit(dif, hitForce);
                _canDamage = false;
                HandleHit();
            }
        }
        
        protected virtual Vector2 GetDirection(Collider2D other) {
            return (other.transform.position - transform.position).normalized;
        }

        protected virtual void HandleHit() { }
        protected virtual void HandleCombatRestart() { }
    }
}