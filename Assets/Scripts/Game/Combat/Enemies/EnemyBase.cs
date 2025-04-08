
using AppCore.AudioManagement;

using AppCore;
using AppCore.FreezeFrameManagement;
using Game.Combat.Bear;
using UnityEngine;
using Tools;

namespace Game.Combat.Enemies {
    public abstract class EnemyBase : MonoBehaviour, IBearHittable {
        public GameObject GameObject => gameObject;

        [SerializeField] private int health = 100;
        [SerializeField] internal int sanityRestored = 10;
        [SerializeField] internal AnimationCurve stunKnockbackCurve;
        [SerializeField] internal GameObject stunObject;
        [SerializeField] internal float stunDuration;
        
        [SerializeField] private GameObject hitParticles;
        [SerializeField] private SoundEffect hitSound;
        [SerializeField] private GameObject deathParticles;
        [SerializeField] private SoundEffect deathSound;
        
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
            stunObject.SetActive(false);
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

        private void HandleDeath() {
            CombatManager.EnemyKilled(this);
            
            CurrentState.Die();
            
            this.CreateParticles(deathParticles);
            CombatManager.cameraShaker.Shake();
            deathSound?.Play();
            App.Get<FreezeFrameManager>().FreezeFrame();
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
            
            if (health <= 0) {
                HandleDeath();
            }
            
            if (damage > 0) {
                this.CreateParticles(hitParticles, transform.position, hitDirection);
                hitSound.Play();
            }
        }

        internal void DestroyEnemy() {
            CombatManager.RemoveEnemy(this);
            CurrentState.Die();
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(Tags.EnemyDestroyer)) {
                DestroyEnemy();
            }
        }
    }
}