using System;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class TreeAppendage : MonoBehaviour, IBearHittable {
        public GameObject GameObject => gameObject;

        [SerializeField] private TreeEnemy enemyBase;
        [SerializeField] private GameObject hitParticles;
        
        public void OnHitByBear(int damage, Vector2 hitDirection, float knockbackForce, BearDamageType damageType) {
            enemyBase.HandleAppendageHit(damage, hitDirection, knockbackForce, damageType);
            // this.CreateParticles(hitParticles, transform.position, hitDirection);
        }
    }
}