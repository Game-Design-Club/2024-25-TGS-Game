using Tools;
using UnityEngine;

namespace Game.Combat.Bear {
    public class Pounce : BearState {
        public Pounce(BearController controller) : base(controller) { }

        private float _startRotation;
        private Vector2 _startDirection;
        
        private bool _swipePressed = false;
        private bool _pounceInputReleased = false;
        
        private float _time = 0;
        
        public override void Enter() {
            _startDirection = Controller.LastDirection;
            _startRotation = Controller.LastRotation;
            Controller.Animator.SetTrigger(AnimationParameters.Bear.Pounce);
            Controller.mainCollider.enabled = false;
            Controller.pounceHitbox.SetActive(true);
        }
        
        public override void Exit() {
            Controller.mainCollider.enabled = true;
            Controller.pounceHitbox.SetActive(false);
        }

        public override float? GetWalkSpeed() {
            return Controller.pounceSpeed;
        }
        
        public override Vector2 GetWalkDirection() {
            return _startDirection;
        }
        
        public override float? GetRotation() {
            return _startRotation;
        }
        
        public override void OnSwipeInput() {
            _swipePressed = true;
        }
        
        public override void OnPounceInputReleased() {
            _pounceInputReleased = true;
        }

        public override void Update() {
            _time += Time.deltaTime;
            if (_time >= Controller.maxPounceLength) EndState();
            if (_pounceInputReleased && _time >= Controller.minPounceLength) EndState();
            return;
            
            void EndState() {
                if (_swipePressed) {
                    Controller.TransitionToState(new Swipe(Controller));
                } else {
                    Controller.TransitionToState(new Idle(Controller));
                }
            }
        }
    }
}