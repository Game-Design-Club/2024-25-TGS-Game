using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class Reach : EnemyState {
        public Reach(EnemyBase controller) : base(controller) { }
        
        public override void Update() {
            Controller<TreeEnemy>().ChangeDistance(Controller<TreeEnemy>().reachSpeed * Time.deltaTime);
        }

        public override void OnHit(Vector2 hitDirection, float hitForce) {
            Controller().TransitionToState(new Retract(Controller()));
        }
    }
}