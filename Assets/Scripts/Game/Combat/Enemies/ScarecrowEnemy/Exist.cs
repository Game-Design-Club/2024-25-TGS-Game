using System.Collections;
using System.Collections.Generic;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.ScarecrowEnemy {
    public class Exist : EnemyState {
        private static int _numJumpAttacks = 0;
        private static int _numMurderAttacks = 0;
        
        private float _attackBufferTime;
        
        public Exist(EnemyBase controller, float attackBuffer) : base(controller) {
            _attackBufferTime = attackBuffer;
        }
        
        public override void Enter() {
            Controller().StartCoroutine(WaitToAttack());
        }

        private IEnumerator WaitToAttack() {
            yield return new WaitForSeconds(_attackBufferTime);
            if (_numJumpAttacks >= Controller<Scarecrow>().maxConsecutiveJumps) {
                _numJumpAttacks = 0;
                Controller().TransitionToState(new MurderAttack(Controller()));
            } else if (_numMurderAttacks >= Controller<Scarecrow>().maxConsectiveMurders) {
                _numMurderAttacks = 0;
                Controller().TransitionToState(new JumpAttack(Controller()));
            } else if (Random.value > 0.5f) {
                _numJumpAttacks++;
                Controller().TransitionToState(new JumpAttack(Controller()));
            } else {
                _numMurderAttacks++;
                Controller().TransitionToState(new MurderAttack(Controller()));
            }
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            
        }
    }
}