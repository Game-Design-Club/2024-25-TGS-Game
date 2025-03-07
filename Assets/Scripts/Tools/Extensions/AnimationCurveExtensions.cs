using UnityEngine;

namespace Tools {
    public static class AnimationCurveExtensions {
        public static Keyframe LastKey(this AnimationCurve curve) {
            return curve.keys[curve.length - 1];
        }

        public static float Time(this AnimationCurve curve) {
            return curve.keys[curve.length - 1].time;
        }

        public static float Percentage(this AnimationCurve curve, float time) {
            return time / curve.Time();
        }
    }
}