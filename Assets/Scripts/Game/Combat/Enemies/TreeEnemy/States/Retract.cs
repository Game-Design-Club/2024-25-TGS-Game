using Game.Combat.Bear;
using Tools;
using Tools.Extensions;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class Retract : EnemyState {
        public Retract(EnemyBase controller) : base(controller) { }

        private float _timer = 0;
        private float _startDistance = 0;

        public override void Enter() {
            Controller().Animator.SetTrigger(Constants.Animator.TreeEnemy.Reset);
            _startDistance = Controller<TreeEnemy>().CurrentDistance;
        }

        public override void Exit() {
            Controller<TreeEnemy>().CreateNewPoints();
        }

        public override void Update() {
            _timer += Time.deltaTime;
            AnimationCurve drawbackCurve = Controller().stunKnockbackCurve;
            Controller<TreeEnemy>().SetDistance(_startDistance - drawbackCurve.Evaluate(_timer));
            if (_timer > drawbackCurve.Time()) {
                Controller().TransitionToState(new Reach(Controller()));
            }
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            Controller().TransitionToState(new Retract(Controller()));
        }
    }
}