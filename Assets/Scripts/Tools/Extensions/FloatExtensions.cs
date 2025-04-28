namespace Tools {
    public static class FloatExtensions {
        public static float Random(this float value) {
            return UnityEngine.Random.Range(-value, value);
        }
    }
}