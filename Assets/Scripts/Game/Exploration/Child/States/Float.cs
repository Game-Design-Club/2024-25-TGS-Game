using Game.Exploration.Enviornment.River;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class Float : ChildState {
        public Float(ChildController controller) : base(controller) { }
        
        RiverChunk _currentChunk;
        private bool _doneFloating = false;

        public override void Enter() {
            Controller.StopAllCoroutines();
            Controller.Animator.SetBool(AnimationParameters.Child.Float, true);
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, true);
            Update();
        }

        public override void Update() {
            if (_doneFloating) return;
            PointCollision pointCollision = Controller.NewPointCollision;
            if (pointCollision.TouchingGround || pointCollision.TouchingLog) {
                Controller.StartMoveUntilGrounded();
                _doneFloating = true;
            } else if (pointCollision.TouchingRiver) {
                _currentChunk = pointCollision.River;
            }
        }

        public override float? GetRotation() {
            return 270; // Down
        }

        public override float? GetWalkSpeed() {
            return _currentChunk?.floatSpeed;
        }

        public override Vector2 GetWalkDirection() {
            return _currentChunk.Direction;
        }

        public override void Exit() {
            Controller.Animator.SetBool(AnimationParameters.Child.Float, false);
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, false);
        }
    }
}