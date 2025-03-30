using Game.Combat.Bear;
using UnityEngine;

namespace Game.Combat.Enemies {
    public abstract class EnemyState {
        private readonly EnemyBase _controller; // onoly use getcontroller to get, never use it directly
        
        // ReSharper disable Unity.PerformanceAnalysis
        protected T Controller<T>() where T : EnemyBase {
            if (_controller is T controller) {
                return controller;
            }

            Debug.LogError($"Controller is not of type {typeof(T)}");
            return null;
        }
        
        protected EnemyBase Controller() => _controller;

        protected EnemyState(EnemyBase controller) {
            _controller = controller;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }
        public virtual void Update() { }

        public abstract void OnHit(Vector2 hitDirection, float hitForce, BearDamageType damageType);
        internal void HandleHit(Vector2 hitDirection, float hitForce, BearDamageType damageType, EnemyState callbackState) {
            float stunDuration = 0;
            if (damageType is BearDamageType.Growl or BearDamageType.Pounce) {
                stunDuration = _controller.stunDuration;
            }
            Controller().TransitionToState(new Stun(Controller(), hitDirection, hitForce, stunDuration,callbackState));
        }
        
        public virtual void OnAnimationEnded() { }

        public virtual void Die() {
            Object.Destroy(_controller.gameObject);
        }
    }
}