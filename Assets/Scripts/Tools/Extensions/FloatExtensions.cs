namespace Tools {
    public static class FloatExtensions {
        public static float GetRandom(this float value) {
            return UnityEngine.Random.Range(-value, value);
        }
    }
}