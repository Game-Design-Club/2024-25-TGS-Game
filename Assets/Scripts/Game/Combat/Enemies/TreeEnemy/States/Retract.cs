using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.TreeEnemy {
    public class Retract : EnemyState {
        public Retract(EnemyBase controller) : base(controller) {
            Controller<TreeEnemy>().stunObject.SetActive(false);
        }
        public Retract(EnemyBase controller, float multiplier, float extraStunTime) : base(controller) {
            _stunMultiplier = multiplier;
            _extraStunTime = extraStunTime;
            if (_extraStunTime > 0) {
                Controller<TreeEnemy>().stunObject.SetActive(true);
            }
        }
        
        private float _stunMultiplier = 1;
        private float _extraStunTime = 0;

        private float _timer = 0;
        private float _startDistance = 0;

        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationConstants.TreeEnemy.Reset);
            _startDistance = Controller<TreeEnemy>().CurrentDistance;
        }

        public override void Exit() {
            Controller<TreeEnemy>().CreateNewPoints();
        }

        public override void Update() {
            _timer += Time.deltaTime;
            AnimationCurve drawbackCurve = Controller().stunKnockbackCurve;
            Controller<TreeEnemy>().SetDistance(_startDistance - drawbackCurve.Evaluate(_timer) * _stunMultiplier);
            if (Controller<TreeEnemy>().GetDistance() < Controller<TreeEnemy>().dieRange) {
                Controller<TreeEnemy>().Die();
            }
            if (_timer > drawbackCurve.Time()) {
                if (_extraStunTime > 0) {
                    Controller().TransitionToState(new Stunned(Controller(), _extraStunTime));
                } else {
                    Controller().TransitionToState(new Reach(Controller()));
                }
            }
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            Controller().TransitionToState(new Retract(Controller()));
        }
    }
}