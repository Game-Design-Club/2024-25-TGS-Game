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

        public float Percentage(float percentage) {
            return Min + (Max - Min) * percentage;
        }

        public float PercentageOpp(float percentage) {
            return Max - (Max - Min) * percentage;
        }

        public float Lerp(float percentage) {
            return Mathf.Lerp(Min, Max, percentage);
        }
    }
}