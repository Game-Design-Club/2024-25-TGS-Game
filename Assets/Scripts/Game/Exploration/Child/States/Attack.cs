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
            Controller.Animator.SetTrigger(AnimationConstants.Child.Attack);
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