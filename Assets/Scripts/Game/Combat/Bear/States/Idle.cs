
namespace Game.Combat.Bear {
    public class Idle : BearState {
        public Idle(BearController controller) : base(controller) { }
        
        public override void OnSwipeInput() {
            Controller.TransitionToState(new Swipe(Controller));
        }
    }
}