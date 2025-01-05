using System;

namespace Tools {
    [Serializable]
    public struct IntRange {
        public int Min;
        public int Max;
        
        public IntRange(int min, int max) {
            Min = min;
            Max = max;
        }
        
        public int Random() {
            if (Min == Max) return Min;
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}