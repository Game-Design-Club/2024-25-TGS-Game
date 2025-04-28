using AppCore;
using AppCore.AudioManagement;
using AppCore.FreezeFrameManagement;
using Game.Combat.Bear;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.ScarecrowEnemy {
    public class Scarecrow : EnemyBase {
        [SerializeField] internal float moveAttackBufferTime = 0.5f;
        [SerializeField] internal float murderAttackBufferTime = 4f;
        [SerializeField] internal GameObject[] limbs;
        [SerializeField] internal Collider2D[] limbColliders;
        [SerializeField] internal float[] limbsHealthCutoffs;
        [Header("Jump")]
        [SerializeField] internal AnimationCurve jumpCurve;
        [SerializeField] internal float jumpDistance;
        [Header("Crow")]
        [SerializeField] internal GameObject crowPrefab;
        [SerializeField] internal IntRange murderCount = new IntRange(5, 8);
        [SerializeField] internal FloatRange murderSpawnDelay = new FloatRange(0.02f, 0.2f);
        [SerializeField] internal float murderSpawnOffset = 1;
        [SerializeField] internal float topSpawnOffset;
        
        private int _limbIndex = 0;

        protected override EnemyState StartingState => new Exist(this, moveAttackBufferTime);
        
        public void OnStartJump() {
            if (CurrentState is MoveAttack moveAttack) {
                moveAttack.StartJump();
            }
        }

        internal override void ProcessHitInternal(Vector2 hitDirection, float hitForce, BearDamageType damageType) {
            if (_limbIndex >= limbs.Length) return;
            if (!(health < limbsHealthCutoffs[_limbIndex])) return;
            
            limbs[_limbIndex].SetActive(false);
            limbColliders[_limbIndex].enabled = false;
            Instantiate(deathParticles, limbs[_limbIndex].transform.position, Quaternion.identity);
            App.Get<AudioManager>().PlaySoundEffect(deathSound);
            App.Get<FreezeFrameManager>().FreezeFrame();
            _limbIndex++;
        }
    }
}