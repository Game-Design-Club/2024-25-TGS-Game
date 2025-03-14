using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Enemies.AttackEnemy {
    public class AttackEnemy : EnemyBase {
        [FormerlySerializedAs("walkSpeed")] [SerializeField] internal float moveSpeed = 1;
        [SerializeField] internal Transform rotationPivot;
        [SerializeField] internal SpriteRenderer spriteRenderer;
        protected override EnemyState StartingState => new Move(this);

        internal bool StartingFlipX;
        
        private void Awake() {
            TryGetComponent(out Animator);
            TryGetComponent(out Rigidbody);
            StartingFlipX = spriteRenderer.flipX;
        }
        
        internal void OnEntityEnterTrigger(Collider2D other) {
            if (other.CompareTag(Tags.Child) || other.CompareTag(Tags.Bear)) {
                TransitionToState(new Attack(this));
            }
        }
    }
}