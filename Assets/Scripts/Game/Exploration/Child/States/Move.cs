using Game.GameManagement;
using UnityEngine;

namespace Game.Exploration.Child {
    public class Move : ChildState {
        public Move(ChildController controller) : base(controller) { }

        public override void Enter() {
            Controller.walkSound.paused = () => Controller.LastInput.magnitude <= .1f;
            Controller.walkSound.Play();
        }

        public override void Exit() {
            Controller.walkSound.Stop();
        }
    }
}