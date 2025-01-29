using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Enemies.AttackEnemy {
    public class AttackEnemy : EnemyBase {
        [FormerlySerializedAs("walkSpeed")] [SerializeField] internal float moveSpeed = 1;
        protected override EnemyState StartingState => new Move(this);

        private void Awake() {
            TryGetComponent(out Animator);
            TryGetComponent(out Rigidbody);
        }
        
        internal void OnEntityEnterTrigger(Collider2D other) {
            if (other.CompareTag(Constants.Tags.Child) || other.CompareTag(Constants.Tags.Bear)) {
                TransitionToState(new Attack(this));
            }
        }
        
        protected override void HandleHitChild() {
            CurrentState.Die();
            CombatManager.RemoveEnemy(this);
        }
    }
}