using System.Linq;
using UnityEngine;

namespace Game.Combat.Waves {
    [CreateAssetMenu(fileName = "Wave", menuName = "Combat/Wave", order = 0)]
    public class Wave : ScriptableObject {
        [SerializeField] public WaveEntry[] waveEntries;
        [SerializeField] public bool canReplay;
        
        public int GetTotalEnemies() {
            return waveEntries.Sum(entry => entry.GetSpawnTimes().Count);
        }
    }
}