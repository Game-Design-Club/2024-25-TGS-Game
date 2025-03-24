using System.Collections;
using AppCore;
using AppCore.InputManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class Jump : ChildState {
        public Jump(ChildController controller) : base(controller) { }

        private float _t = 0;
        private bool _jumpReleased = false;
        
        public override void Enter() {
            Controller.Animator.SetBool(AnimationConstants.Child.Jump, true);
            if (!App.Get<InputManager>().GetChildJump) {
                _jumpReleased = true;
            }
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, true);
        }

        public override void Update() {
            _t += Time.deltaTime;
            if (_jumpReleased && _t >= Controller.minJumpTime) {
                Controller.TransitionToState(new Move(Controller));
                return;
            }

            if (_t >= Controller.maxJumpTime) {
                Controller.TransitionToState(new Move(Controller));
                return;
            }
        }

        public override void OnJumpInputReleased() {
            _jumpReleased = true;
        }

        public override bool CanInteract() {
            return false;
        }

        public override float? GetWalkSpeed() {
            return Controller.jumpSpeed;
        }

        public override void Exit() {
            Controller.Animator.SetBool(AnimationConstants.Child.Jump, false);
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, false);
            
        }
    }
}