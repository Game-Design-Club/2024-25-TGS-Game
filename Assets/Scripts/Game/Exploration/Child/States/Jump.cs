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
        
        public override void Enter() {
            Controller.Animator.SetBool(AnimationConstants.Child.Jump, true);
            if (!App.Get<InputManager>().GetChildJump) {
                _jumpReleased = true;
            }
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, true);
            _jumpDirection = Controller.LastDirection;
        }

        public override void Update() {
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
            Vector2 pos = Controller.Rigidbody.position;
            float xSize = Controller.boxCollider.size.x / 2;
            float ySize = Controller.boxCollider.size.y / 2;
            
            PlayerPointCollision topLeft = new PlayerPointCollision(pos + new Vector2(-xSize, ySize));
            PlayerPointCollision topRight = new PlayerPointCollision(pos + new Vector2(xSize, ySize));
            PlayerPointCollision bottomLeft = new PlayerPointCollision(pos + new Vector2(-xSize, -ySize));
            PlayerPointCollision bottomRight = new PlayerPointCollision(pos + new Vector2(xSize, -ySize));
            
            if (topLeft.TouchingRiver && topRight.TouchingRiver && bottomLeft.TouchingRiver && bottomRight.TouchingRiver) {
                Controller.TransitionToState(new Float(Controller));
                return;
            }
            if (topLeft.TouchingGround && topRight.TouchingGround && bottomLeft.TouchingGround && bottomRight.TouchingGround) {
                Controller.TransitionToState(new Move(Controller));
                return;
            }
            Controller.TransitionToState(new Move(Controller));
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