using System;
using System.Collections;
using Game.GameManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Exploration.Enviornment {
    public class GlobalLighting : MonoBehaviour {
        [SerializeField] private float combatIntensity;
        [SerializeField] private float exploreIntensity;
        
        private Light2D _light;
        
        private void Awake() {
            TryGetComponent(out _light);
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
                    StartCoroutine(TransitionLighting(combatIntensity));
                    break;
                case GameEventType.ExploreEnter:
                case GameEventType.Cutscene:
                    StartCoroutine(TransitionLighting(exploreIntensity));
                    break;
                case GameEventType.Combat:
                case GameEventType.Explore:
                    break;
            }
        }

        private IEnumerator TransitionLighting(float p1) {
            if (Mathf.Approximately(p1, _light.intensity)) yield break;
            float p0 = _light.intensity;
            float t = 0;
            while (t < GameManager.TransitionDuration) {
                t += Time.deltaTime;
                _light.intensity = Mathf.Lerp(p0, p1, t / GameManager.TransitionDuration);
                yield return null;
            }
        }
    }
}