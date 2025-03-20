using Game.Combat.Bear;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class Stunned : EnemyState {
        public Stunned(EnemyBase controller, float time) : base(controller) {
            _targetTime = time;
        }
        
        private float _targetTime;
        private float _progress = 0;

        public override void Enter() {
            Controller<TreeEnemy>().stunObject.SetActive(true);
        }

        public override void Exit() {
            Controller<TreeEnemy>().stunObject.SetActive(false);
        }

        public override void Update() {
            _progress += Time.deltaTime;
            
            if (_progress >= _targetTime) {
                Controller().TransitionToState(new Reach(Controller()));
            }
        }

        public override void OnHit(Vector2 hitDirection, float knockbackForce, BearDamageType damageType) {
            
        }
    }
}