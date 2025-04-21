using Tools;
using UnityEngine;

namespace Game.Exploration.Child
{
    public class Attack : ChildState
    {
        private float _startRotation;
        
        public Attack(ChildController controller) : base(controller) {
            _startRotation = controller.LastRotation;
        }

        public override void Enter()
        {
            Controller.Animator.SetTrigger(AnimationParameters.Child.Attack);
            Controller.swishParticles.Play();
        }

        public override void Exit() {
            Controller.swishParticles.Stop();
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
            return _startRotation;
        }

        public override void OnAttackAnimationOver()
        {
            Controller.TransitionToState(new Move(Controller));
        }
    }
}