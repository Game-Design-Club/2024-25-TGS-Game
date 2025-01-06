using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
using Tools;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Exploration.Enviornment {
    public class Campfire : MonoBehaviour {
        [Header("Lit")]
        [SerializeField] private float litIntensity;
        [SerializeField] private float litRadius;
        [Header("Unlit")]
        [SerializeField] private float unlitIntensity;
        [SerializeField] private float unlitRadius;
        [Header("Flicker")]
        [SerializeField] private FlickerData flickerData;
        private float _exploreIntensity;
        
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;

        private void Awake() {
            TryGetComponent(out _spriteRenderer);
            TryGetComponent(out _light);
            _exploreIntensity = _light.intensity;
        }

        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
        }

        private void OnGameEvent(GameEvent gameEvent) {
            switch (gameEvent.GameEventType) {
                case GameEventType.CombatEnter:
                case GameEventType.Bear:
                    StartCoroutine(Flicker());
                    break;
                case GameEventType.ExploreEnter:
                case GameEventType.Child:
                case GameEventType.Cutscene:
                    StopAllCoroutines();
                    _light.intensity = _exploreIntensity;
                    break;
            }
        }
        
        private IEnumerator Flicker() {
            _light.intensity = litIntensity;
            _light.pointLightOuterRadius = litRadius;
            
            while (true) {
                yield return new WaitForSeconds(flickerData.pause.Random());
                for (int i = 0; i < flickerData.count.Random(); i++) {
                    _light.intensity = unlitIntensity;
                    _light.pointLightOuterRadius = unlitRadius;
                    yield return new WaitForSeconds(flickerData.duration.Random());
                    _light.intensity = litIntensity;
                    _light.pointLightOuterRadius = litRadius;
                    yield return new WaitForSeconds(flickerData.interval.Random());
                }
            }
        }
    }

    [Serializable]
    public struct FlickerData {
        public IntRange count;
        public FloatRange duration;
        public FloatRange interval;
        public FloatRange pause;
    }
}