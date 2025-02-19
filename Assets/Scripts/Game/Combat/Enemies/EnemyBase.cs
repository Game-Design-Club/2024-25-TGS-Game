using UnityEngine;

namespace Game.Combat.Enemies {
    public abstract class EnemyBase : MonoBehaviour, IBearHittable {
        public GameObject GameObject => gameObject;

        [SerializeField] private int health = 100;
        [SerializeField] internal int sanityRestored = 10;
        [SerializeField] internal int sanityDamage = 10;
        [SerializeField] internal AnimationCurve stunKnockbackCurve;
        
        internal CombatAreaManager CombatManager;
        
        internal EnemyState CurrentState;
        internal Animator Animator;
        internal Rigidbody2D Rigidbody;
        protected abstract EnemyState StartingState { get; }

        private void Awake() {
            TryGetComponent(out Animator);
            TryGetComponent(out Rigidbody);
        }

        private protected void Start() {
            TransitionToState(StartingState);
        }

        internal void TransitionToState(EnemyState newState) {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        
        internal void ProcessHit(Vector2 hitDirection, float knockbackForce) {
            CurrentState?.OnHit(hitDirection, knockbackForce);
        }
        protected void OnAnimationEnded() {
            CurrentState.OnAnimationEnded();
        }
        protected void HandleDeath() {
            CurrentState.Die();
        }
        private void Update() {
            CurrentState?.Update();
        }

        public void OnHit(int damage, Vector2 hitDirection, float knockbackForce) {
            TakeDamage(damage, hitDirection, knockbackForce);
        }
        
        public void TakeDamage(int damage, Vector2 hitDirection, float knockbackForce) {
            health -= damage;
            
            ProcessHit(hitDirection, knockbackForce);
            
            if (health <= 0) {
                CombatManager.EnemyKilled(this);
                HandleDeath();
            }
        }

        public void HitChild() {
            CombatManager.ChildHit(this);
            HandleHitChild();
        }

        protected virtual void HandleHitChild() {
            CombatManager.RemoveEnemy(this);
            CurrentState.Die();
        }
    }
}