using System;
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
        private Vector2 _jumpDirection;
        private bool _doneJumping = false;
        
        public override void Enter() {
            Controller.Animator.SetBool(AnimationConstants.Child.Jump, true);
            if (!App.Get<InputManager>().GetChildJump) {
                _jumpReleased = true;
            }
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, true);
            _jumpDirection = Controller.LastDirection;
        }

        public override void Update() {
            if (_doneJumping) return;
            _t += Time.deltaTime;
            if (_jumpReleased && _t >= Controller.minJumpTime) {
                DoneJumping();
                return;
            }

            if (_t >= Controller.maxJumpTime) {
                DoneJumping();
                return;
            }
        }
        
        private void DoneJumping() {
            _doneJumping = true;
            Controller.LandPlayer();
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
        
        public override Vector2 GetWalkDirection() {
            return _jumpDirection;
        }

        public override void Exit() {
            Controller.Animator.SetBool(AnimationConstants.Child.Jump, false);
        }
    }
}