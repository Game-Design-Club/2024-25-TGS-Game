using UnityEngine;

namespace Game.Combat {
    [System.Serializable]
    public struct Wave {
        [SerializeField] internal GameObject enemyPrefab;
        [SerializeField] internal Transform spawnPoint;
    }
}