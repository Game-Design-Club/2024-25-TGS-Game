using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class FinalEncounterState : ChildState {
        public FinalEncounterState(ChildController controller) : base(controller) {
        }

        private bool _sleeping = false;
        
        public override void Enter() {
            Controller.Animator.SetBool(AnimationParameters.Child.Sleep, false);
            
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, false);
            Controller.walkSound.paused = () => Controller.LastInput.magnitude <= .1f;
            Controller.walkSound.Play();
            
            Controller.spriteRenderer.sortingLayerName = RenderingLayers.BearGameplay;

            Controller.combatBoxCollider.enabled = false;
            Controller.chaseBoxCollider.enabled = true;
        }

        public override void Sleep(Vector3 position) {
            Controller.Animator.SetBool(AnimationParameters.Child.Sleep, true);
            _sleeping = true;
        }
        
        public override float? GetWalkSpeed() {
            if (_sleeping) return 0;
            else return Controller.walkSpeed;
        }

        public override void OnAttackInput() {
            Controller.TransitionToState(new Attack(Controller, new FinalEncounterState(Controller)));
        }

        public override void Exit() {
            Controller.spriteRenderer.sortingLayerName = RenderingLayers.ChildGameplay;
            Controller.combatBoxCollider.enabled = true;
            Controller.chaseBoxCollider.enabled = false;
        }
    }
}