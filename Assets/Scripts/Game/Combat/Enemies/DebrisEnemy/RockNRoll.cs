using System.Collections;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.DebrisEnemy {
    public class RockNRoll : EnemyState {
        private bool _horizontalMovement;

        public RockNRoll(EnemyBase controller, bool horizontalMovement) : base(controller) {
            _horizontalMovement = horizontalMovement;
        }

        private Vector2 _direction;
        private float _speed;
        
        private Vector2 _stunAddition;
        
        public override void Enter() {
            _speed = Controller<DebrisEnemy>().rollSpeed;

            float randomness = Controller<DebrisEnemy>().directionAngleVariance;
            float randomAdd = randomness.Random();

            Vector2 dif = Controller().Child.transform.position - Controller().transform.position;
            float angle = _horizontalMovement 
                ? Vector2.SignedAngle(Vector2.right, dif) 
                : Vector2.SignedAngle(Vector2.down, dif);   
            
            if (angle > Controller<DebrisEnemy>().maxAngle) {
                angle = Controller<DebrisEnemy>().maxAngle;
            } else if (angle < -Controller<DebrisEnemy>().maxAngle) {
                angle = -Controller<DebrisEnemy>().maxAngle;
            }
                
            angle += randomAdd;
            _direction = Quaternion.Euler(0, 0, angle) * (_horizontalMovement ? Vector2.right : Vector2.down);
        }
        
        public override void Update() {
            Controller().Rigidbody.linearVelocity = _direction * _speed + _stunAddition;
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            Controller().StopAllCoroutines();
            Controller().StartCoroutine(Stun(hitDirection, hitForce));
        }

        private IEnumerator Stun(Vector2 hitDirection, float hitForce) {
            float t = 0;
            float hitXChange = hitDirection.x * Controller<DebrisEnemy>().hitDirectionChange;
            
            while (t < Controller().stunKnockbackCurve.Time()) {
                t += Time.deltaTime;
                _stunAddition = hitDirection * (Controller().stunKnockbackCurve.Evaluate(t) * hitForce * Controller<DebrisEnemy>().hitWeight);
                _stunAddition.x += hitXChange;
                yield return null;
            }
            _stunAddition = Vector2.zero;
            
            _direction.x += hitXChange;
            _direction.Normalize();
        }
    }
}