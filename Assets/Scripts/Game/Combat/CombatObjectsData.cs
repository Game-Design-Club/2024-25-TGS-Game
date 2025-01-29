using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat {
    [CreateAssetMenu(fileName = "CombatObjectsData", menuName = "Combat/CombatObjectsData", order = 0)]
    public class CombatObjectsData : ScriptableObject {
        [FormerlySerializedAs("basicEnemyAttackerPrefab")] [SerializeField] private GameObject attackerPrefab;
        [SerializeField] private GameObject shooterPrefab;
        private static CombatObjectsData Instance => _instance ??= Resources.Load<CombatObjectsData>("CombatObjectsData");
        private static CombatObjectsData _instance;

        public static GameObject GetEnemyPrefab(EnemyType enemyType) {
        return enemyType switch {
                EnemyType.Attacker => Instance.attackerPrefab,
                EnemyType.Shooter => Instance.shooterPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null)
            };
        }
    }

    public enum EnemyType {
        Attacker,
        Shooter
    }
}