using AppCore;
using AppCore.DataManagement;
using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class Move : ChildState {
        public Move(ChildController controller) : base(controller) { }

        public override void OnAttackInput() {
            if (App.Get<DataManager>().GetFlag(BoolFlags.HasStick)) {
                Controller.TransitionToState(new Attack(Controller));
            }
        }

        public override void OnJumpInput() {
            // if (App.Get<DataManager>().GetFlag(BoolFlags.HasJump)) {
            Controller.TransitionToState(new Jump(Controller));
        //}
        }

        public override void Enter() {
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, false);
            Controller.walkSound.paused = () => Controller.LastInput.magnitude <= .1f;
            Controller.walkSound.Play();
        }

        public override void Exit() {
            Controller.walkSound.Stop();
        }
    }
}