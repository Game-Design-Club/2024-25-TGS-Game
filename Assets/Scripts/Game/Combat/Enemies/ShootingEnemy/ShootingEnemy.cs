using Game.Combat.Enemies.States;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Enemies {
    public class ShootingEnemy : EnemyBase {
        [FormerlySerializedAs("walkSpeed")] [SerializeField] internal float moveSpeed = 1f;
        [SerializeField] internal float attackDistance = 4f;
        [SerializeField] internal float shootInterval = 1f;
        [SerializeField] internal float bulletSpeed = 1.5f;
        [SerializeField] private Transform shootSpawnPoint;
        [SerializeField] internal GameObject bulletPrefab;

        protected override EnemyState StartingState => new ShootAndMove(this);

        public void Shoot() {
            GameObject bullet = Instantiate(bulletPrefab, shootSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyDamageDealer>().enemyBase = this;
            Vector2 posDifference = -(transform.position - CombatManager.Child.transform.position);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = posDifference.normalized * bulletSpeed;

        }
    }
}