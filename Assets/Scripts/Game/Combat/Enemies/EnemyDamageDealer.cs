using System;
using Game.Combat.Bear;
using Game.GameManagement;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Enemies {
    public class EnemyDamageDealer : MonoBehaviour {
        [SerializeField] private float hitForce = 10;
        [SerializeField] internal EnemyBase enemyBase;
        [SerializeField] private bool hitChild = true;
        [SerializeField] private bool hitBear = true;
        [SerializeField] internal float sanityDamage = 10;
        [FormerlySerializedAs("killChildOnAttack")] [SerializeField] private bool killEnemyOnChildAttack = true;
        [SerializeField] private bool killEnemyOnBearAttack = true;
        
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
            if (hitChild && other.CompareTag(Tags.Child)) {
                if (enemyBase != null && killEnemyOnChildAttack) {
                    enemyBase.DestroyEnemy();
                }
                HandleHit();
                if (_combatManager) {
                    _combatManager.ChildHit(this);
                } else {
                    Debug.LogWarning("Child Hit aaaaaaa");
                }
            }
            if (hitBear && _canDamage && other.TryGetComponent(out BearEnemyHitbox bearEnemyHitbox)) {
                if (enemyBase != null && killEnemyOnBearAttack) {
                    enemyBase.DestroyEnemy();
                }
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