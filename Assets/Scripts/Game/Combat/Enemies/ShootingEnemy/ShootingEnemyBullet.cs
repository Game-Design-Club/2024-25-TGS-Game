namespace Game.Combat.Enemies {
    public class ShootingEnemyBullet : EnemyDamageDealer {
        protected override void HandleHit() {
            Destroy(gameObject);
        }

        public void Destroy() {
            Destroy(gameObject);
        }
    }
}