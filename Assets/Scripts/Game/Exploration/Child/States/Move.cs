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
            if (App.Get<DataManager>().GetFlag(BoolFlags.HasJump)) {
                Controller.TransitionToState(new Jump(Controller));
            }
        }

        public override void Enter() {
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, false);
            Controller.walkSound.paused = () => Controller.LastInput.magnitude <= .1f;
            Controller.walkSound.Play();
        }

        public override void Exit() {
            Controller.walkSound.Stop();
        }

        public override void Update() {
            float xSize = (Controller.mainBoxCollider.size.x / 2) * Controller.mainBoxCollider.transform.lossyScale.x;
            float ySize = (Controller.mainBoxCollider.size.y / 2) * Controller.mainBoxCollider.transform.lossyScale.y;       
            float xOffset = Controller.mainBoxCollider.offset.x * Controller.mainBoxCollider.transform.lossyScale.x;
            float yOffset = Controller.mainBoxCollider.offset.y * Controller.mainBoxCollider.transform.lossyScale.y;
            Vector2 offset = new Vector2(xOffset, yOffset);
            int i = 0;
            Vector2 pos = Controller.Rigidbody.position;
            Vector2 topLeftPos = pos + new Vector2(-xSize, ySize) + offset;
            Vector2 topRightPos = pos + new Vector2(xSize, ySize) + offset;
            Vector2 bottomLeftPos = pos + new Vector2(-xSize, -ySize) + offset;
            Vector2 bottomRightPos = pos + new Vector2(xSize, -ySize) + offset;
            
            PointCollision topLeft = new PointCollision(topLeftPos, Controller.mainBoxCollider);
            PointCollision topRight = new PointCollision(topRightPos, Controller.mainBoxCollider);
            PointCollision bottomLeft = new PointCollision(bottomLeftPos, Controller.mainBoxCollider);
            PointCollision bottomRight = new PointCollision(bottomRightPos, Controller.mainBoxCollider);
            
            if (!topLeft.TouchingLand && !topRight.TouchingLand && !bottomLeft.TouchingLand && !bottomRight.TouchingLand) {
                Controller.TransitionToState(new Float(Controller));
                return;
            }
            
        }
    }
}