using AppCore;
using AppCore.FreezeFrameManagement;
using UnityEngine;
using Tools;

namespace Game.Combat.Enemies {
    public abstract class EnemyBase : MonoBehaviour, IBearHittable {
        public GameObject GameObject => gameObject;

        [SerializeField] private int health = 100;
        [SerializeField] internal int sanityRestored = 10;
        [SerializeField] internal AnimationCurve stunKnockbackCurve;
        
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private ParticleSystem deathParticles;
        
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
        
        internal void ProcessHit(Vector2 hitDirection, float knockbackForce, BearDamageType damageType) {
            CurrentState?.OnHit(hitDirection, knockbackForce, damageType);
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

        public void OnHitByBear(int damage, Vector2 hitDirection, float knockbackForce, BearDamageType damageType) {
            TakeDamage(damage, hitDirection, knockbackForce, damageType);
        }
        
        public void TakeDamage(int damage, Vector2 hitDirection, float knockbackForce, BearDamageType damageType) {
            health -= damage;
            
            ProcessHit(hitDirection, knockbackForce, damageType);

            if (damage > 0) {
                this.CreateParticles(hitParticles, transform.position, hitDirection);
            }
            
            if (health <= 0) {
                CombatManager.EnemyKilled(this);
                HandleDeath();
                
                this.CreateParticles(deathParticles);
                CombatManager.cameraShaker.Shake();
                App.Get<FreezeFrameManager>().FreezeFrame(0.1f, .2f);
            }
        }

        internal void HitChild() {
            CombatManager.RemoveEnemy(this);
            CurrentState.Die();
        }
    }
}