using UnityEngine;

namespace Game.Combat.Enemies {
    public class ShootingEnemyBullet : EnemyDamageDealer, IBearHittable {
        [SerializeField] private float sideKnockbackWeight = .3f;
        public GameObject GameObject => gameObject;

        protected override Vector2 GetDirection(Collider2D other) {
            Vector2 dif = (other.transform.position - transform.position).normalized;
            Vector2 dir = GetComponent<Rigidbody2D>().linearVelocity.normalized;
            return (dir + dif * sideKnockbackWeight).normalized;
        }

        protected override void HandleHit() {
            Destroy();
        }
        
        protected override void HandleCombatRestart() {
            Destroy();
        }

        public void Destroy() {
            Destroy(gameObject);
        }


        public void OnBearHit(int damage, Vector2 hitDirection, float knockbackForce) {
            Destroy();
        }
    }
}