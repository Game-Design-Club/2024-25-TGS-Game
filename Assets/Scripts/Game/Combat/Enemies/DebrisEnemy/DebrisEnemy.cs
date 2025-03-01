using System.Numerics;
using UnityEngine;

namespace Game.Combat.Enemies.DebrisEnemy {
    public class DebrisEnemy : EnemyBase {
        [SerializeField] internal float maxAngle = 30;
        [SerializeField] internal float directionAngleVariance = 10;
        [SerializeField] internal float rollSpeed = 5;
        [SerializeField] internal float hitWeight = .3f;

        protected override EnemyState StartingState => new RockNRoll(this);
    }
}