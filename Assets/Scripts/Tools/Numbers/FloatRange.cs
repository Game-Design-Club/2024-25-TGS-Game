using System;

namespace Tools {
    [Serializable]
    public struct FloatRange {
        public float Min;
        public float Max;
        
        public FloatRange(float min, float max) {
            Min = min;
            Max = max;
        }
        
        public float Random() {
            if (Math.Abs(Min - Max) < 0.000001) return Min;
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}