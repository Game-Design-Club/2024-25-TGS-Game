using System;
using Game.GameManagement;
using UnityEngine;

namespace Game.Combat.Enemies.CrowEnemy {
    public class CrowEnemy : EnemyBase {
        [SerializeField] internal float maxAngle = 30;
        [SerializeField] internal float directionAngleVariance = 10;
        [SerializeField] internal float attackSpeed = 5;

        protected override EnemyState StartingState => new GoWheeee(this);
    }
}