using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;

namespace Game.Tutorial {
    public class TutorialPopupObject : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI textObj;
        [SerializeField] private float animationTransitionDuration = 0.5f;

        private Animator _animator;

        private bool _isShowing;

        private static TutorialPopupObject _instance;

        private void Awake() {
            TryGetComponent(out _animator);
            _instance = this;
        }

        public static void ShowTutorialPopup(string text) {
            if (_instance == null || _instance._animator == null)
                return;

            _instance.Display(text);
        }
        
        public static void HideTutorialPopup() {
            if (_instance == null || _instance._animator == null)
                return;

            _instance.Hide();
        }
        
        private void Display(string next) {
            textObj.text = next;
            _animator.SetBool(AnimationParameters.Tutorial.Popup, true);
        }
        
        private void Hide() {
            _animator.SetBool(AnimationParameters.Tutorial.Popup, false);
        }
    }
}