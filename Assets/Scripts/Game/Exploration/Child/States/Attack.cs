using Tools;
using UnityEngine;

namespace Game.Exploration.Child
{
    public class Attack : ChildState
    {
        private float _startRotation;
        
        private ChildState _endState;
        
        public Attack(ChildController controller) : base(controller) {
            _startRotation = controller.LastRotation;
        }

        public Attack(ChildController controller, ChildState backState) : base(controller) {
            _startRotation = controller.LastRotation;
            _endState = backState;
        }

        public override void Enter()
        {
            Controller.Animator.SetTrigger(AnimationParameters.Child.Attack);
            Controller.spriteAnimator.SetBool(AnimationParameters.ChildSprites.Attack, true);
            Controller.swishParticles.Play();
        }

        public override void Exit() {
            Controller.swishParticles.Stop();
            Controller.spriteAnimator.SetBool(AnimationParameters.ChildSprites.Attack, false);
        }

        public override void Update() {
            if (new PointCollision(Controller.swishMatchTransform.position).TouchingGround) {
                Controller.swishParticles.Play();
            } else {
                Controller.swishParticles.Stop();
            }
            Controller.swishParticles.transform.position = Controller.swishMatchTransform.position;
            float rotation = Controller.LastRotation +
                             (Controller.swishParticles.transform.lossyScale.x < 0 ? 180 : 0);
            Controller.swishParticles.transform.rotation = Quaternion.Euler(0, 0, rotation); 
        }

        public override float? GetWalkSpeed()
        {
            return Controller.walkSpeed * .25f;
        }

        public override float? GetRotation() {
            Vector2 facingDir = new Vector2(
                Mathf.Cos(Mathf.Deg2Rad * _startRotation),
                Mathf.Sin(Mathf.Deg2Rad * _startRotation)
            );
            Controller.spriteAnimator.SetFloat(AnimationParameters.ChildSprites.MoveX, facingDir.x);
            Controller.spriteAnimator.SetFloat(AnimationParameters.ChildSprites.MoveY, facingDir.y);
            return _startRotation;
        }

        public override void OnAttackAnimationOver() {
            Controller.TransitionToState(_endState ?? new Move(Controller));
        }
    }
}