using Game.Exploration.Enviornment.River;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class Float : ChildState {
        public Float(ChildController controller) : base(controller) { }
        
        RiverChunk _currentChunk;

        public override void Enter() {
            Controller.Animator.SetBool(AnimationConstants.Child.Float, true);
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, true);
            Update();
        }

        public override void Update() {
            if (Controller.PointCollision.TouchingRiver) {
                _currentChunk = Controller.PointCollision.River;
            } else {
                Controller.TransitionToState(new Move(Controller));
            }
        }

        public override float? GetWalkSpeed() {
            return Controller.floatSpeed;
        }

        public override Vector2 GetWalkDirection() {
            return _currentChunk.GetDirection;
        }

        public override void Exit() {
            Controller.Animator.SetBool(AnimationConstants.Child.Float, false);
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, false);
        }
    }
}