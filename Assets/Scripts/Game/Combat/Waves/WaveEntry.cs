using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Game.Combat.Waves {
    [Serializable]
    public class WaveEntry {
        [SerializeField] public AnimationCurve spawnRate;
        [SerializeField] public EnemyType enemyType;
        [SerializeField] public bool spawnLeft = true;
        [SerializeField] public bool spawnRight = true;
        [SerializeField] public bool spawnTop = true;
        [SerializeField] public bool spawnBottom = true;
        [SerializeField] public float bufferAfterThisEntry = 0;
        [SerializeField] public int customData = 0;

        private List<float> _spawnTimes = null;
        private static float _valueTarget = 1f;
        private static float _timeStep = 0.1f;

        
        // Uses numerical integration to calculate the times to spawn enemies
        // Numerical integration goes through the spawn rate curveto find the area under a non-formulaic curve
        public List<float> GetSpawnTimes() {
            _spawnTimes = new List<float>();
            float totalTime = spawnRate.LastKey().time;
            float lastTime = 0;
            float time = 0;
            float value = 0;
            
            _spawnTimes.Add(0);
            
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