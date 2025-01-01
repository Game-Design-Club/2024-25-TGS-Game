using System;
using Game.GameManagement;
using Tools;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Exploration.Enviornment {
    public class Campfire : MonoBehaviour {
        [SerializeField] private Color unlitColor;
        private Color litColor;
        
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;

        private void Awake() {
            TryGetComponent(out _spriteRenderer);
            TryGetComponent(out _light);
            litColor = _light.color;
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
                    _light.enabled = false;
                    _spriteRenderer.color = unlitColor;
                    break;
                case GameEventType.ExploreEnter:
                case GameEventType.Child:
                case GameEventType.Cutscene:
                    _light.enabled = true;
                    _spriteRenderer.color = litColor;
                    break;
            }
        }
    }
}