using System;
using System.Numerics;
using Tools;
using UnityEngine;

namespace Game.Combat.Enemies.DebrisEnemy {
    public class DebrisEnemy : EnemyBase {
        [SerializeField] internal float maxAngle = 30;
        [SerializeField] internal float directionAngleVariance = 10;
        [SerializeField] internal float rollSpeed = 5;
        [SerializeField] internal float hitWeight = .3f;
        [SerializeField] internal float hitDirectionChange = .5f;

        public bool horizontalMovement = false;

        protected override EnemyState StartingState => new RockNRoll(this, horizontalMovement);
    }
}