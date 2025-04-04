using System.Collections;
using System.Collections.Generic;
using Tools;
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

        protected override EnemyState StartingState => new ShootAndMove(this);

        public void Shoot() {
            GameObject bullet = Instantiate(bulletPrefab, shootSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyDamageDealer>().enemyBase = this;
            Vector2 posDifference = -(transform.position - CombatManager.Child.transform.position);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = posDifference.normalized * bulletSpeed;
        }

        public void StartShootCycle() {
            StartCoroutine(WaitOneFrameAndStartShootCycle());
        }
        
        private IEnumerator WaitOneFrameAndStartShootCycle() {
            if (CurrentState == null) {
                yield return null;
            }

            if (CurrentState is ShootAndMove shootAndMove) {
                shootAndMove.StartShootCycle();
            }
        }
    }
}