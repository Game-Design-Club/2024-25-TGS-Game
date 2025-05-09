using System;
using System.Collections;
using Game.GameManagement;
using UnityEngine;

namespace Game.Combat {
    public class SanityBar : MonoBehaviour {
        [SerializeField] private GameObject sanityBar;
        [SerializeField] private RectTransform sanityFill;
        [SerializeField] private Color fullColor;
        [SerializeField] private Color emptyColor;
        [SerializeField] private float moveTowardsSpeed = 1f;
        
        private float _targetSanity;
        private float _sanity;

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
            _targetSanity = sanity;
        }

        private void Update() {
            if (Math.Abs(_sanity - _targetSanity) > 0.01f) {
                _sanity = Mathf.MoveTowards(_sanity, _targetSanity, moveTowardsSpeed * Time.deltaTime);
            } else {
                _sanity = _targetSanity;
            }
            sanityFill.localScale = new Vector3(1, _sanity, 1);
            sanityFill.GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(emptyColor, fullColor, _sanity);
        }

        private void OnGameEvent(GameEvent gameEvent) {
            switch (gameEvent.GameEventType) {
                case GameEventType.Combat:
                case GameEventType.CombatEnter:
                    sanityBar.gameObject.SetActive(true);
                    break;
                default:
                    sanityBar.gameObject.SetActive(false);
                    break;
            }
        }
    }
}