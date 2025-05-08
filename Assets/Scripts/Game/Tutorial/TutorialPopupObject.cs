using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;

namespace Game.Tutorial {
    public class TutorialPopupObject : MonoBehaviour {
        [Header("Assign in Inspector")]
        [SerializeField] private TextMeshProUGUI textObj;
        [Header("Animation timing (match your Animator!)")]
        [SerializeField] private float animationTransitionDuration = 0.5f;

        private Animator _animator;

        // our queue of pending messages
        private readonly Queue<string> _popupQueue = new Queue<string>();
        // are we currently showing a popup?
        private bool _isShowing;
        // are we already in the middle of animating a switch?
        private bool _isTransitioning;

        private static TutorialPopupObject _instance;

        private void Awake() {
            TryGetComponent(out _animator);
            _instance = this;
        }

        /// <summary>
        /// Call this from anywhere to enqueue a tutorial message.
        /// The very first one shows immediately;
        /// subsequent ones will animate out/in as they take over.
        /// </summary>
        public static void ShowTutorialPopup(string text) {
            if (_instance == null || _instance._animator == null)
                return;

            _instance._popupQueue.Enqueue(text);

            // if nothing is up yet, bring up this one right away
            if (!_instance._isShowing && !_instance._isTransitioning) {
                _instance.DisplayNext();
            }
            // otherwise, start a transition (if one isn't already happening)
            else if (!_instance._isTransitioning) {
                _instance.StartCoroutine(_instance.HideAndShowNext());
            }
        }

        /// <summary>
        /// Call this when you want to hide the current popup and clear the queue.
        /// </summary>
        public static void HideTutorialPopup() {
            if (_instance == null || _instance._animator == null || !_instance._isShowing || _instance._isTransitioning)
                return;

            _instance.StartCoroutine(_instance.HideAndClearQueue());
        }

        private void DisplayNext() {
            if (_popupQueue.Count == 0) {
                _isShowing = false;
                return;
            }

            // dequeue and show
            string next = _popupQueue.Dequeue();
            textObj.text = next;
            _animator.SetBool(AnimationParameters.Tutorial.Popup, true);
            _isShowing = true;
        }

        private IEnumerator HideAndShowNext() {
            _isTransitioning = true;

            // animate down
            _animator.SetBool(AnimationParameters.Tutorial.Popup, false);
            yield return new WaitForSeconds(animationTransitionDuration);

            // display the next one in queue
            DisplayNext();

            _isTransitioning = false;
        }

        private IEnumerator HideAndClearQueue() {
            _isTransitioning = true;

            // animate down
            _animator.SetBool(AnimationParameters.Tutorial.Popup, false);
            yield return new WaitForSeconds(animationTransitionDuration);

            // reset everything
            _popupQueue.Clear();
            _isShowing = false;
            _isTransitioning = false;
        }
    }
}