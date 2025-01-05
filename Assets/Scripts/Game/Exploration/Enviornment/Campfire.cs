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
        [SerializeField] private Color unlitColor;
        [SerializeField] private FlickerData flickerData;
        private Color _litColor;
        
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;

        private void Awake() {
            TryGetComponent(out _spriteRenderer);
            TryGetComponent(out _light);
            _litColor = _light.color;
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
                    _light.enabled = true;
                    _spriteRenderer.color = _litColor;
                    break;
            }
        }
        
        private IEnumerator Flicker() {
            while (true) {
                for (int i = 0; i < flickerData.count.Random(); i++) {
                    _light.enabled = false;
                    _spriteRenderer.color = unlitColor;
                    yield return new WaitForSeconds(flickerData.duration.Random());
                    _light.enabled = true;
                    _spriteRenderer.color = _litColor;
                    yield return new WaitForSeconds(flickerData.interval.Random());
                }
                
                yield return new WaitForSeconds(flickerData.pause.Random());
            }
        }
    }

    [System.Serializable]
    public struct FlickerData {
        public IntRange count;
        public FloatRange duration;
        public FloatRange interval;
        public FloatRange pause;
    }
}