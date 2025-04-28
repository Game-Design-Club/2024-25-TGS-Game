using System.Collections;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.ScarecrowEnemy {
    public class MurderAttack : EnemyState {
        public MurderAttack(EnemyBase controller) : base(controller) {
        }
        
        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationParameters.ScarecrowEnemy.Crow);
        }
        
        public override void OnAnimationEnded() {
            Controller().StartCoroutine(SpawnCrows());
        }
        
        private IEnumerator SpawnCrows() {
            Vector2 baseSpawnPos = new Vector2(
                Controller().transform.position.x,
                Controller().CombatManager.Top + Controller<Scarecrow>().topSpawnOffset);
            for (int i = 0; i < Controller<Scarecrow>().murderCount.Random(); i++) {
                GameObject crow = Object.Instantiate(Controller<Scarecrow>().crowPrefab);
                crow.transform.position = baseSpawnPos +
                                          new Vector2(Controller<Scarecrow>().murderSpawnOffset.Random(), 0);
                crow.GetComponent<EnemyBase>().CombatManager = Controller().CombatManager;
                yield return new WaitForSeconds(Controller<Scarecrow>().murderSpawnDelay.Random());
            }
            Controller().TransitionToState(new Exist(Controller(), Controller<Scarecrow>().murderAttackBufferTime));
        }
        
        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            
        }
    }
}