using System;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class TreeAppendage : MonoBehaviour, IBearHittable {
        public GameObject GameObject => gameObject;

        [SerializeField] private TreeEnemy enemyBase;
        
        public void OnBearHit(int damage, Vector2 hitDirection, float knockbackForce) {
            enemyBase.Hit();
        }
    }
}