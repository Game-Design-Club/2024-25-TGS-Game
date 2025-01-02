using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat {
    [CreateAssetMenu(fileName = "WavesData", menuName = "Combat/WavesData", order = 0)]
    public class WavesData : ScriptableObject {
        [SerializeField] public Wave[] waves;
        [SerializeField] public float bufferBetweenWaves;
    }
    
    [CreateAssetMenu(fileName = "Wave", menuName = "Combat/Wave", order = 0)]
    public class Wave : ScriptableObject {
        [SerializeField] public WaveEntry[] waveEntries;
        [SerializeField] public bool canReplay;
        
        public int GetTotalEnemies() {
            return waveEntries.Sum(entry => entry.GetSpawnTimes().Count);
        }
    }
    
    [Serializable]
    public struct WaveEntry {
        [SerializeField] public AnimationCurve spawnRate;
        [SerializeField] public EnemyType enemyType;
        [SerializeField] public bool spawnLeft;
        [SerializeField] public bool spawnRight;
        [SerializeField] public bool spawnTop;
        [SerializeField] public bool spawnBottom;
        [SerializeField] public float offscreenSpawnDistance ;

        private List<float> _spawnTimes;
        private static float _valueTarget = 1f;
        private static float _timeStep = 0.1f;
        
        public List<float> GetSpawnTimes() {
            if (_spawnTimes.Count > 0) return _spawnTimes;
            _spawnTimes = new List<float>();
            float totalTime = spawnRate.keys[spawnRate.length - 1].time;
            float lastTime = 0;
            float time = 0;
            float value = 0;
            
            _spawnTimes.Add(0);
            
            // wooo calculus
            while (time < totalTime) {
                time += _timeStep;
                value += spawnRate.Evaluate(time) * _timeStep;
                while (value >= _valueTarget) {
                    _spawnTimes.Add(time - lastTime);
                    value -= _valueTarget;
                    lastTime = time;
                }
            }
            return _spawnTimes;
        }
    }

}