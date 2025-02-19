using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class Reach : EnemyState {
        public Reach(EnemyBase controller) : base(controller) { }
        
        private Vector2 lastChildPosition = Vector2.zero;
        
        public override void Update() {
            Vector2 childPosition = Controller<TreeEnemy>().CombatManager.Child.transform.position;
            if (lastChildPosition != childPosition) {
                Controller<TreeEnemy>().CreateNewPoints();
            }
            lastChildPosition = childPosition;
            Controller<TreeEnemy>().ChangeDistance(Controller<TreeEnemy>().reachSpeed * Time.deltaTime);
        }

        public override void OnHit(Vector2 hitDirection, float hitForce) {
            Controller().TransitionToState(new Retract(Controller()));
        }
    }
}