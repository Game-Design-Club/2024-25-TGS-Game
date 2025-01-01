using System;
using Game.GameManagement;
using UnityEngine;

namespace Game.Combat {
    public class SanityBar : MonoBehaviour {
        [SerializeField] private GameObject sanityBar;
        [SerializeField] private RectTransform sanityFill;

        private void Start() {
            sanityBar.gameObject.SetActive(false);
        }

        private void OnEnable() {
            CombatAreaManager.OnSanityChanged += UpdateSanityBar;
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            CombatAreaManager.OnSanityChanged -= UpdateSanityBar;
            GameManager.OnGameEvent -= OnGameEvent;
        }
        
        private void UpdateSanityBar(float sanity) {
            sanityFill.localScale = new Vector3(sanity, 1, 1);
        }

        private void OnGameEvent(GameEvent gameEvent) {
            switch (gameEvent.GameEventType) {
                case GameEventType.Bear:
                case GameEventType.CombatEnter:
                    sanityBar.gameObject.SetActive(true);
                    break;
                case GameEventType.ExploreEnter:
                case GameEventType.Cutscene:
                case GameEventType.Child:
                    sanityBar.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}