using System.Collections;
using System.Collections.Generic;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.ScarecrowEnemy {
    public class Exist : EnemyState {
        private float _attackBufferTime;
        
        public Exist(EnemyBase controller, float attackBuffer) : base(controller) {
            _attackBufferTime = attackBuffer;
        }
        
        public override void Enter() {
            Controller().StartCoroutine(WaitToAttack());
        }

        private IEnumerator WaitToAttack() {
            yield return new WaitForSeconds(_attackBufferTime);
            if (Random.value > 0.5f) {
                Controller().TransitionToState(new MoveAttack(Controller()));
            } else {
                Controller().TransitionToState(new MurderAttack(Controller()));
            }
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            
        }
    }
}