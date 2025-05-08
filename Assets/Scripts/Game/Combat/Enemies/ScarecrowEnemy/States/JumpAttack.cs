using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.ScarecrowEnemy {
    public class JumpAttack : EnemyState {
        
        private float _totalTime;
        private float _time = 0;
        
        Vector2 _startPos;
        Vector2 _endPos;
        
        private bool _jumping = false;

        private bool _stun = false;
        private float _stunDuration;
        
        public JumpAttack(EnemyBase controller) : base(controller) {
            _startPos = Controller().transform.position;
            Vector2 childDif = -(Controller().transform.position - Controller().Child.transform.position).normalized;
            _endPos = (Vector2)controller.transform.position + childDif * Controller<Scarecrow>().jumpDistance;
        }
        
        public override void Enter() {
            Controller().Animator.SetTrigger(AnimationParameters.ScarecrowEnemy.Jump);
        }

        public override void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            _stun = true;
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
            if (_stun) {
                Controller().TransitionToState(new Stun(Controller(), Vector2.zero, 0, Controller<Scarecrow>().stunDuration, new Exist(Controller(), Controller<Scarecrow>().moveAttackBufferTime)));
                return;
            }
            Controller().TransitionToState(new Exist(Controller(), Controller<Scarecrow>().moveAttackBufferTime));
            Object.Instantiate(Controller<Scarecrow>().landParticles, Controller().transform.position, Quaternion.identity);
        }

        public void StartJump() {
            _jumping = true;
            _totalTime = Controller().Animator.GetCurrentAnimatorStateInfo(0).length - _time;
            _time = 0;
        }
    }
}