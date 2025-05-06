using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class FinalEncounterState : ChildState {
        public FinalEncounterState(ChildController controller) : base(controller) {
        }
        
        public override void Enter() {
            Controller.Animator.SetBool(AnimationParameters.Child.Sleep, false);
            
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, false);
            Controller.walkSound.paused = () => Controller.LastInput.magnitude <= .1f;
            Controller.walkSound.Play();
        }
    }
}