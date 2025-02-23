using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class Stunned : EnemyState {
        public Stunned(EnemyBase controller) : base(controller) { }
        
        private float _progress = 0;
        
        public override void Update() {
            _progress += Time.deltaTime;
            
            if (_progress >= Controller<TreeEnemy>().stunDuration) {
                Controller().TransitionToState(new Reach(Controller()));
            }
        }

        public override void OnHit(Vector2 hitDirection, float knockbackForce, BearDamageType damageType) {
        }
    }
}