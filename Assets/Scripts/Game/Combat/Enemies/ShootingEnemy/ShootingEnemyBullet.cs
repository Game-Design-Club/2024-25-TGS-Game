using UnityEngine;

namespace Game.Combat.Enemies {
    public class ShootingEnemyBullet : EnemyDamageDealer {
        [SerializeField] private float sideKnockbackWeight = .3f;
        protected override Vector2 GetDirection(Collider2D other) {
            Vector2 dif = (other.transform.position - transform.position).normalized;
            Vector2 dir = GetComponent<Rigidbody2D>().linearVelocity.normalized;
            return (dir + dif * sideKnockbackWeight).normalized;
        }

        protected override void HandleHit() {
            Destroy(gameObject);
        }

        public void Destroy() {
            Destroy(gameObject);
        }
    }
}