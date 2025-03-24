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
            Vector2 pos = Controller.Rigidbody.position;
            float xSize = Controller.boxCollider.size.x / 2;
            float ySize = Controller.boxCollider.size.y / 2;
            
            PlayerPointCollision topLeft = new PlayerPointCollision(pos + new Vector2(-xSize, ySize));
            PlayerPointCollision topRight = new PlayerPointCollision(pos + new Vector2(xSize, ySize));
            PlayerPointCollision bottomLeft = new PlayerPointCollision(pos + new Vector2(-xSize, -ySize));
            PlayerPointCollision bottomRight = new PlayerPointCollision(pos + new Vector2(xSize, -ySize));
            
            if (DoAll(point => point.TouchingGround)) {
                Controller.TransitionToState(new Move(Controller));
                return;
            }
            
            if (DoAny(point => point.TouchingLog) || DoAny(point => point.TouchingRock)) {
                Controller.StartMoveUntilGrounded();
                return;
            }
            
            if (DoAll(point => point.TouchingRiver)) {
                Controller.TransitionToState(new Float(Controller));
                return;
            }
            
            Controller.StartMoveUntilGrounded();
            
            bool DoAll(Func<PlayerPointCollision, bool> predicate) {
                return predicate(topLeft) && predicate(topRight) && predicate(bottomLeft) && predicate(bottomRight);
            }

            bool DoAny(Func<PlayerPointCollision, bool> predicate) {
                return predicate(topLeft) || predicate(topRight) || predicate(bottomLeft) || predicate(bottomRight);
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
        
        public override Vector2 GetWalkDirection() {
            return _jumpDirection;
        }

        public override void Exit() {
            Controller.Animator.SetBool(AnimationConstants.Child.Jump, false);
        }
    }
}