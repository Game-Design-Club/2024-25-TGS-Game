using System.Collections;
using UnityEngine;

namespace Game.Combat.Enemies.States {
    internal class ShootAndMove : EnemyState {
        public ShootAndMove(EnemyBase controller) : base(controller) { }

        private Transform _targetTransform;
        
        public override void Enter() {
            _targetTransform = Controller().CombatManager.Child.transform;
            Controller().StartCoroutine(Shoot());
        }

        private IEnumerator Shoot() {
            while (true) {
                yield return new WaitForSeconds(Controller<ShootingEnemy>().shootInterval);
                Controller<ShootingEnemy>().Shoot();
            }
        }

        public override void Update() {
            Vector2 posDifference = -(Controller().transform.position - _targetTransform.position);
            if (posDifference.magnitude > Controller<ShootingEnemy>().attackDistance) {
                Controller().Rigidbody.linearVelocity = posDifference.normalized * Controller<ShootingEnemy>().moveSpeed;
            }
            Controller().Rigidbody.rotation = Mathf.Atan2(posDifference.y, posDifference.x) * Mathf.Rad2Deg;
        }

        public override void OnHit(Vector2 hitDirection, float hitForce) {
            HandleHit(hitDirection, hitForce, new ShootAndMove(Controller()));
        }
    }
}