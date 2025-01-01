using System;
using UnityEngine;

namespace Game.Combat {
    [CreateAssetMenu(fileName = "CombatObjectsData", menuName = "Combat/CombatObjectsData", order = 0)]
    public class CombatObjectsData : ScriptableObject {
        [SerializeField] private GameObject basicEnemyAttackerPrefab;
        private static CombatObjectsData Instance => _instance ??= Resources.Load<CombatObjectsData>("CombatObjectsData");
        private static CombatObjectsData _instance;

        public static GameObject GetEnemyPrefab(EnemyType enemyType) {
            Debug.Log(Instance);
            return enemyType switch {
                EnemyType.BasicEnemyAttacker => Instance.basicEnemyAttackerPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null)
            };
        }
    }

    public enum EnemyType {
        BasicEnemyAttacker
    }
}