using UnityEngine;

namespace Tools {
    [System.Serializable]
    public struct FloatRange {
        public float Min;
        public float Max;
        
        public FloatRange(float min, float max) {
            Min = min;
            Max = max;
        }
        
        public float Random() {
            if (Mathf.Abs(Min - Max) < 0.000001) return Min;
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}