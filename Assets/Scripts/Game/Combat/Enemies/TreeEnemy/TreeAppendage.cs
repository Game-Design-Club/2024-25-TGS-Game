using System;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class TreeAppendage : MonoBehaviour, IBearHittable {
        public GameObject GameObject => gameObject;

        [SerializeField] private TreeEnemy enemyBase;
        
        public void OnHitByBear(int damage, Vector2 hitDirection, float knockbackForce, BearDamageType damageType) {
            enemyBase.HandleAppendageHit(damage, hitDirection, knockbackForce, damageType);
        }
    }
}