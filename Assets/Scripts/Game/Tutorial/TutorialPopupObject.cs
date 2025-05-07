using System;
using TMPro;
using Tools;
using UnityEngine;

namespace Game.Tutorial {
    public class TutorialPopupObject : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI textObj;
        
        private Animator _animator;

        private static TutorialPopupObject _instance;
        
        private void Awake() {
            TryGetComponent(out _animator);
            _instance = this;
        }

        public static void ShowTutorialPopup(string text) {
            _instance.textObj.text = text;
            _instance._animator.SetBool(AnimationParameters.Tutorial.Popup, true);
        }
        
        public static void HideTutorialPopup() {
            _instance._animator.SetBool(AnimationParameters.Tutorial.Popup, false);
        }
    }
}