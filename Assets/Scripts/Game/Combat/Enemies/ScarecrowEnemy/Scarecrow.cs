using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.ScarecrowEnemy {
    public class Scarecrow : EnemyBase {
        [SerializeField] internal float moveAttackBufferTime = 0.5f;
        [SerializeField] internal float murderAttackBufferTime = 4f;
        [Header("Jump")]
        [SerializeField] internal AnimationCurve jumpCurve;
        [SerializeField] internal float jumpDistance;
        [Header("Crow")]
        [SerializeField] internal GameObject crowPrefab;
        [SerializeField] internal IntRange murderCount = new IntRange(5, 8);
        [SerializeField] internal FloatRange murderSpawnDelay = new FloatRange(0.02f, 0.2f);
        [SerializeField] internal float murderSpawnOffset = 1;
        [SerializeField] internal float topSpawnOffset;

        protected override EnemyState StartingState => new Exist(this, moveAttackBufferTime);
        
        public void OnStartJump() {
            if (CurrentState is MoveAttack moveAttack) {
                moveAttack.StartJump();
            }
        }
    }
}