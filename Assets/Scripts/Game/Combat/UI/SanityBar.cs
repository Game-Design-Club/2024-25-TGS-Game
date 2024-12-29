using System;
using UnityEngine;

namespace Game.Combat {
    public class SanityBar : MonoBehaviour {
        [SerializeField] private RectTransform sanityBar;
        
        private void OnEnable() {
            CombatAreaManager.OnSanityChanged += UpdateSanityBar;
        }
        
        private void OnDisable() {
            CombatAreaManager.OnSanityChanged -= UpdateSanityBar;
        }
        
        private void UpdateSanityBar(float sanity) {
            sanityBar.localScale = new Vector3(sanity, 1, 1);
        }
    }
}