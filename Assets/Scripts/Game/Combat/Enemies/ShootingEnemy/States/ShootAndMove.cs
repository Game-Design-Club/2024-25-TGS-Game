using System.Collections;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies {
    internal class ShootAndMove : EnemyState {
        public ShootAndMove(EnemyBase controller) : base(controller) { }

        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationParameters.ShootEnemy.Teleport);
            Controller<ShootingEnemy>().rotatePivot.gameObject.SetActive(false);
        }

        public override void Exit() {
            Controller().StopAllCoroutines();
        }

        private IEnumerator Shoot() {
            yield return new WaitForSeconds(Controller<ShootingEnemy>().shootWaitTime);
            Controller<ShootingEnemy>().Shoot();
            yield return new WaitForSeconds(Controller<ShootingEnemy>().teleportWaitTime);
            Controller().Animator.SetTrigger(AnimationParameters.ShootEnemy.Teleport);
        }

        private void FindNewLocation() {
            Vector2 childPos = Controller().Child.transform.position;
            float angle = Random.Range(Controller<ShootingEnemy>().minAngle, Controller<ShootingEnemy>().maxAngle);
            if (!Mathf.Approximately(Controller<ShootingEnemy>().minAngle, Controller<ShootingEnemy>().maxAngle + 360)) {
                 angle = Mathf.Lerp(
                    Controller<ShootingEnemy>().minAngle,
                    Controller<ShootingEnemy>().maxAngle,
                    (Random.value + Random.value) * 0.5f
                );
            }
            Vector2 randomDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            float randomDistance = Controller<ShootingEnemy>().spawnDistance.Random();
            Vector2 newLocation = childPos + randomDirection * randomDistance;
    
            Controller().Rigidbody.position = newLocation;
            
            Vector2 posDifference = childPos - Controller().Rigidbody.position;
            Controller<ShootingEnemy>().rotatePivot.rotation = Quaternion.Euler(0,0,Mathf.Atan2(posDifference.y, posDifference.x) * Mathf.Rad2Deg);
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            if (damageType is BearDamageType.Swipe) {
                Controller().TransitionToState(new ShootAndMove(Controller()));
            } else {
                HandleHit(hitDirection, hitForce, damageType, new ShootAndMove(Controller()));
            }
        }

        public void StartShootCycle() {
            FindNewLocation();
            Controller().StartCoroutine(Shoot());
        }
    }
}