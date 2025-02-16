using System;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class TreeAppendage : MonoBehaviour, IBearHittable {
        [SerializeField] private SpriteRenderer handSprite;
        public GameObject GameObject => gameObject;

        [SerializeField] private TreeEnemy enemyBase;
        
        public void OnHit(int damage, Vector2 hitDirection, float knockbackForce) {
            enemyBase.Hit();
        }
        
        public void Update() {
            // transform.localScale.x = transform.position.x > enemyBase.CombatManager.Child.transform.position.x;
            transform.localScale = new Vector3(transform.position.x > enemyBase.CombatManager.Child.transform.position.x ? -1 : 1, 1, 1);
        }
    }
}