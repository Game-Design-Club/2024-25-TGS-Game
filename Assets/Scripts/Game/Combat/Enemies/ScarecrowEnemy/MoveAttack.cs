using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.ScarecrowEnemy {
    public class MoveAttack : EnemyState {
        
        private float _totalTime;
        private float _time = 0;
        
        Vector2 _startPos;
        Vector2 _endPos;
        
        private bool _jumping = false;
        
        public MoveAttack(EnemyBase controller) : base(controller) {
            _startPos = Controller().transform.position;
            Vector2 childDif = -(controller.transform.position - Controller().CombatManager.Child.transform.position).normalized;
            _endPos = (Vector2)controller.transform.position + childDif * Controller<Scarecrow>().jumpDistance;
        }
        
        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationParameters.ScarecrowEnemy.Jump);
            _totalTime = Controller().Animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            
        }

        public override void Update() {
            if (!_jumping) return;
            _time += Time.deltaTime;
            float tPercent = _time / _totalTime;
            Vector2 newPos = Vector2.Lerp(_startPos, _endPos, tPercent);
            newPos.y += Controller<Scarecrow>().jumpCurve.Evaluate(tPercent);
            Controller().transform.position = newPos;
        }

        public override void OnAnimationEnded() {
            Controller().TransitionToState(new Exist(Controller(), Controller<Scarecrow>().moveAttackBufferTime));
        }

        public void StartJump() {
            _jumping = true;
        }
    }
}