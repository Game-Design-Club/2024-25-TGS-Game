using UnityEngine;

namespace Game.Combat.Waves {
    [CreateAssetMenu(fileName = "WavesData", menuName = "Combat/WavesData", order = 0)]
    public class WavesData : ScriptableObject {
        [SerializeField] public Wave[] waves;
        [SerializeField] public float bufferBetweenWaves;
    }
    
}