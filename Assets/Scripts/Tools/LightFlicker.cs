using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tools {
    public class LightFlicker : MonoBehaviour {
        [SerializeField] private float intensity;
        [SerializeField] private float frequency;
        
        private Light2D _light;
        private float _baseLightIntensity;

        private void Awake() {
            if (!TryGetComponent(out _light)) {
                Debug.LogError("Please attach a Light2D component for LightFlicker to work", gameObject);
            }
            _baseLightIntensity = _light.intensity;
        }

        private void Update() {
            _light.intensity = (float)(Math.Sin(Time.time * frequency) * intensity) + _baseLightIntensity;
        }
    }
}