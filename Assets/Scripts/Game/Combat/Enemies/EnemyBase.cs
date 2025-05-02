
using AppCore.AudioManagement;

using AppCore;
using AppCore.FreezeFrameManagement;
using Game.Combat.Bear;
using UnityEngine;
using Tools;

namespace Game.Combat.Enemies {
    public abstract class EnemyBase : MonoBehaviour, IBearHittable {
        public GameObject GameObject => gameObject;

        [SerializeField] internal int health = 100;
        [SerializeField] internal AnimationCurve stunKnockbackCurve;
        [SerializeField] internal GameObject stunObject;
        [SerializeField] internal float stunDuration;
        
        [SerializeField] internal GameObject hitParticles;
        [SerializeField] internal SoundEffect hitSound;
        [SerializeField] internal GameObject deathParticles;
        [SerializeField] internal SoundEffect deathSound;
        
        [SerializeField] internal bool spawnedInCombat;
        
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
            if (stunObject != null) {
                stunObject.SetActive(false);
            }
            TransitionToState(StartingState);
            if (spawnedInCombat) {
                CombatManager.AddEnemy(this);
            }
        }

        internal void TransitionToState(EnemyState newState) {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        
        internal void ProcessHit(Vector2 hitDirection, float knockbackForce, BearDamageType damageType) {
            CurrentState?.OnHit(hitDirection, knockbackForce, damageType);
            ProcessHitInternal(hitDirection, knockbackForce, damageType);
        }
        
        internal virtual void ProcessHitInternal(Vector2 hitDirection, float knockbackForce, BearDamageType damageType) { }
        
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

        public virtual void OnHitByBear(BearDamageData data) {
            TakeDamage(data);
        }
        
        public void TakeDamage(BearDamageData data) {
            health -= data.Damage;
            
            ProcessHit(data.HitDirection, data.KnockbackForce, data.DamageType);
            
            if (health <= 0) {
                HandleDeath();
            }
            
            if (data.Damage > 0) {
                this.CreateParticles(hitParticles, data.HitPosition, data.HitDirection);
                hitSound?.Play();
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