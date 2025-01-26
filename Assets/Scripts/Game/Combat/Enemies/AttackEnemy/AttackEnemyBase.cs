using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.AttackEnemy {
    public class AttackEnemyBase : EnemyBase {
        [SerializeField] internal float walkSpeed = 5f;
        
        [SerializeField] internal AnimationCurve stunKnockbackCurve;
        
        internal Animator Animator;
        internal Rigidbody2D Rigidbody;
        
        private AttackEnemyState _currentState;

        private void Awake() {
            TryGetComponent(out Animator);
            TryGetComponent(out Rigidbody);
        }
        
        private void Start() {
            TransitionToState(new Move(this));
        }

        internal void TransitionToState(AttackEnemyState newState) {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
        
        private void Update() {
            _currentState?.Update();
        }

        internal override void ProcessHit(Vector2 hitDirection, float knockbackForce) {
            _currentState?.OnHit(hitDirection, knockbackForce);
        }
        
        protected override void HandleDeath() {
            _currentState.Die();
        }
        
        protected void OnAnimationEnded() {
            _currentState.OnAnimationEnded();
        }

        internal void OnEntityEnterTrigger(Collider2D other) {
            if (other.CompareTag(Constants.Tags.Child) || other.CompareTag(Constants.Tags.Bear)) {
                TransitionToState(new Attack(this));
            }
        }
    }
}