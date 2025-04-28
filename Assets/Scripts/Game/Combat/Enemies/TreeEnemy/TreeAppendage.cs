using System;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class TreeAppendage : MonoBehaviour, IBearHittable {
        public GameObject GameObject => gameObject;

        [SerializeField] private TreeEnemy enemyBase;
        [SerializeField] private GameObject hitParticles;
        
        public void OnHitByBear(BearDamageData data) {
            enemyBase.HandleAppendageHit(data);
            // this.CreateParticles(hitParticles, transform.position, hitDirection);
        }
    }
}