using System;

namespace Tools.CameraShaking {
    [Serializable] public struct CameraShakeData {
        public float intensity;
        public float frequency;
        public float time;
        public bool decay;
        
        public CameraShakeData(float intensity, float frequency, float time, bool decay) {
            this.intensity = intensity;
            this.frequency = frequency;
            this.time = time;
            this.decay = decay;
        }
        public CameraShakeData(CameraShakeData other) {
            intensity = other.intensity;
            frequency = other.frequency;
            time = other.time;
            decay = other.decay;
        }
    }
}