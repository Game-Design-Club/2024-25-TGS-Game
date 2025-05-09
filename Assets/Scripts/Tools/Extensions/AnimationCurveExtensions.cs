using UnityEngine;

namespace Tools {
    public static class AnimationCurveExtensions {
        public static Keyframe LastKey(this AnimationCurve curve) {
            if (curve == null || curve.length == 0) {
                return new Keyframe(0, 0);
            }
            return curve.keys[curve.length - 1];
        }

        public static float Time(this AnimationCurve curve) {
            if (curve == null || curve.length == 0) {
                return 0;
            }
            return curve.keys[curve.length - 1].time;
        }

        public static float Percentage(this AnimationCurve curve, float time) {
            if (curve == null || curve.length == 0) {
                return 0;
            }
            return time / curve.Time();
        }
    }
}