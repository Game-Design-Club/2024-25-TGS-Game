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
        private Vector2 _jumpDirection;
        private bool _doneMoving = false;
        
        public override void Enter() {
            Controller.Animator.SetBool(AnimationConstants.Child.Jump, true);
            Physics2D.IgnoreLayerCollision(Controller.childLayer, Controller.jumpableLayer, true);
            _jumpDirection = Controller.LastDirection;
        }

        public override void Update() {
            _t += Time.deltaTime;
            if (!_doneMoving && _t >= Controller.jumpSpeedCurve.Time()) {
                DoneMoving();
                return;
            }
        }
        
        private void DoneMoving() {
            _doneMoving = true;
            Controller.LandPlayer();
        }

        public override bool CanInteract() {
            return false;
        }

        public override float? GetWalkSpeed() {
            return Controller.jumpSpeedCurve.Evaluate(_t);
        }
        
        public override Vector2 GetWalkDirection() {
            return _jumpDirection;
        }

        public override void Exit() {
            Controller.Animator.SetBool(AnimationConstants.Child.Jump, false);
        }
    }
}