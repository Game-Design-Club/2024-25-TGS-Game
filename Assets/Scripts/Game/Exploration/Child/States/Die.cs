using Tools;

namespace Game.Exploration.Child {
    public class Die : ChildState{
        public Die(ChildController controller) : base(controller) {
        }
        
        public override void Enter() {
            base.Enter();
            Controller.Animator.SetBool(AnimationParameters.Child.IsDead, true);
        }
        
        public override void Exit() {
            base.Exit();
            Controller.Animator.SetBool(AnimationParameters.Child.IsDead, false);
        }
    }
}