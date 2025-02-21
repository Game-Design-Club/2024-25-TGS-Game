using AppCore;
using AppCore.DataManagement;
using Game.GameManagement;

namespace Game.Exploration.Child {
    public class Move : ChildState {
        public Move(ChildController controller) : base(controller) { }

        public override void OnAttackInput()
        {
            if (App.Get<DataManager>().UnlockedAttack) Controller.TransitionToState(new Attack(Controller));
        }
    }
}