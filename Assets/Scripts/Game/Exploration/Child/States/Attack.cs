using UnityEngine;

namespace Game.Exploration.Child
{
    public class Attack : ChildState
    {
        public Attack(ChildController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            Controller.Animator.SetTrigger("Attack");
        }

        public override float? GetWalkSpeed()
        {
            return Controller.walkSpeed * .25f;
        }

        public override void OnAttackAnimationOver()
        {
            Controller.TransitionToState(new Move(Controller));
        }
    }
}