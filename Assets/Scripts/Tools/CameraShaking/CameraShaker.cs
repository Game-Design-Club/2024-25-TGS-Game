using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.CameraShaking {
    public class CameraShaker : MonoBehaviour {
        [SerializeField] private float cutoff = 0.01f;
        [SerializeField] private CameraShakeData defaults;
        [FormerlySerializedAs("baseIntensity")] [SerializeField] private float baseAmplitude = .1f;
        [SerializeField] private float baseFrequency = .1f;
        
        private CinemachineBasicMultiChannelPerlin _cinemachineNoise;

        // Unity functions
        private void Awake() {
            TryGetComponent(out _cinemachineNoise);
        }

        private void Start() {
            _cinemachineNoise.AmplitudeGain = baseAmplitude;
            _cinemachineNoise.FrequencyGain = baseFrequency;
        }

        private void OnDestroy() {
            _cinemachineNoise.AmplitudeGain = 0;
            _cinemachineNoise.FrequencyGain = 0;
        }

        // Private functions
        private void InternalShake(CameraShakeData data) {
            StartCoroutine(ShakeCoroutine(data));
        }
        private IEnumerator ShakeCoroutine(CameraShakeData data) {
            _cinemachineNoise.AmplitudeGain += data.intensity;
            _cinemachineNoise.FrequencyGain += data.frequency;
            if (data.decay) {
                float timer = data.time;
                while (timer > 0) {
                    _cinemachineNoise.AmplitudeGain -= data.intensity * Time.deltaTime / data.time;
                    _cinemachineNoise.FrequencyGain -= data.frequency * Time.deltaTime / data.time;
                    
                    timer -= Time.deltaTime;
                    yield return null;
                }
            } else {
                yield return new WaitForSeconds(data.time);
                _cinemachineNoise.AmplitudeGain -= data.intensity;
                _cinemachineNoise.FrequencyGain -= data.frequency;
            }
            
            if (_cinemachineNoise.AmplitudeGain < baseAmplitude + cutoff) _cinemachineNoise.AmplitudeGain = baseAmplitude;
            if (_cinemachineNoise.FrequencyGain < baseFrequency + cutoff) _cinemachineNoise.FrequencyGain = baseFrequency;
        }
        
        // Public functions
        public void Shake(CameraShakeData data) {
            InternalShake(data);
        }
        public void Shake(float intensity, float frequency, float time, bool decay) {
            Shake(new CameraShakeData(intensity, frequency, time, decay));
        }
        public void Shake() {
            Shake(defaults);
        }
    }
}