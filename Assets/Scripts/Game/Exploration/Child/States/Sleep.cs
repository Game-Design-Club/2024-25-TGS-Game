using System.Collections;
using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class Sleep : ChildState {
        private Vector2 _walkToPoint;
        
        public Sleep(ChildController controller, Vector2 walkToPoint) : base(controller) {
            _walkToPoint = walkToPoint;
        }

        public override void Enter() {
            Controller.StartCoroutine(WalkToPoint());
        }

        public override void Exit() {
            Controller.Animator.SetBool(Constants.Animator.Child.Sleep, false);
        }

        public override float? GetWalkSpeed() {
            return null;
        }

        public override void OnGameEvent(GameEvent gameEvent) {
            if (gameEvent.GameEventType == GameEventType.ExploreEnter) {
                Controller.TransitionToState(new Move(Controller));
            }
        }

        private IEnumerator WalkToPoint() {
            yield return Controller.MoveToPosition(Controller.Rigidbody, _walkToPoint, Controller.walkToPointCurve);
            Controller.Animator.SetBool(Constants.Animator.Child.Sleep, true);
            Controller.Rigidbody.linearVelocity = Vector2.zero;
        }
    }
}