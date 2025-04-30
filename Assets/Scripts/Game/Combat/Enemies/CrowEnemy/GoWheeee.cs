using System.Collections;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.CrowEnemy {
    public class GoWheeee : EnemyState {
        public GoWheeee(EnemyBase controller) : base(controller) { }

        private Vector2 _direction;
        private float _speed;
        
        private Vector2 _stunAddition;
        
        public override void Enter() {
            _speed = Controller<CrowEnemy>().attackSpeed;

            float randomness = Controller<CrowEnemy>().directionAngleVariance;
            float randomAdd = randomness.Random();

            Vector2 dif = Controller().CombatManager.Child.transform.position - Controller().transform.position;
            float angle = Vector2.SignedAngle(Vector2.down, dif);   
            
            if (angle > Controller<CrowEnemy>().maxAngle) {
                angle = Controller<CrowEnemy>().maxAngle;
            } else if (angle < -Controller<CrowEnemy>().maxAngle) {
                angle = -Controller<CrowEnemy>().maxAngle;
            }
            
            angle += randomAdd;
            _direction = Quaternion.Euler(0, 0, angle) * Vector2.down;
        }
        
        public override void Update() {
            Controller().Rigidbody.linearVelocity = _direction * _speed + _stunAddition;
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            HandleHit(hitDirection, hitForce, damageType, new GoWheeee(Controller()));
        }
    }
}