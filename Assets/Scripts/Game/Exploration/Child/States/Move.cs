using AppCore;
using AppCore.DataManagement;
using Game.GameManagement;
using UnityEngine;

namespace Game.Exploration.Child {
    public class Move : ChildState {
        public Move(ChildController controller) : base(controller) { }

        public override void OnAttackInput() {
            if (App.Get<DataManager>().UnlockedAttack) Controller.TransitionToState(new Attack(Controller));
        }

        public override void Enter() {
            Controller.walkSound.paused = () => Controller.LastInput.magnitude <= .1f;
            Controller.walkSound.Play();
        }

        public override void Exit() {
            Controller.walkSound.Stop();
        }
    }
}