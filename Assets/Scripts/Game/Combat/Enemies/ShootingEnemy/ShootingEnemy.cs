using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Enemies {
    public class ShootingEnemy : EnemyBase {
        [FormerlySerializedAs("walkSpeed")] [SerializeField] internal float moveSpeed = 1f;
        [SerializeField] internal float shootWaitTime = .3f;
        [SerializeField] internal float teleportWaitTime = 1f;
        [SerializeField] internal float bulletSpeed = 1.5f;
        [SerializeField] internal FloatRange spawnDistance = new (2f, 5f);
        [SerializeField] private Transform shootSpawnPoint;
        [SerializeField] internal GameObject bulletPrefab;
        [SerializeField] internal Transform rotatePivot;
        [SerializeField] internal float minAngle = 0f;
        [SerializeField] internal float maxAngle = 360f;

        protected override EnemyState StartingState => new ShootAndMove(this);
        
        internal List<GameObject> Bullets = new();

        private void OnEnable() {
            CombatAreaManager.OnClearCombatArea += ClearBullets;
        }
        
        private void OnDisable() {
            CombatAreaManager.OnClearCombatArea -= ClearBullets;
        }

        private void ClearBullets() {
            foreach (GameObject bullet in Bullets) {
                if (bullet) {
                    Destroy(bullet);
                }
            }
            Bullets.Clear();
        }

        public void Shoot() {
            GameObject bullet = Instantiate(bulletPrefab, shootSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyDamageDealer>().enemyBase = this;
            Vector2 posDifference = -(transform.position - Child.transform.position);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = posDifference.normalized * bulletSpeed;
            Bullets.Add(bullet);
        }

        public void StartShootCycle() {
            StartCoroutine(WaitOneFrameAndStartShootCycle());
        }
        
        private IEnumerator WaitOneFrameAndStartShootCycle() {
            if (CurrentState == null) {
                yield return null;
            }
            
            rotatePivot.gameObject.SetActive(true);
            
            
            if (CurrentState is ShootAndMove shootAndMove) {
                shootAndMove.StartShootCycle();
            }
        }
        
        public override void SetCustomData(int entryCustomData) {
            if (entryCustomData == 1) {
                minAngle = 90;
                maxAngle = 270;
            }
        }
    }
}