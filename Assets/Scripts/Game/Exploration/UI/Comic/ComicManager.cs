using System;
using System.Collections;
using System.Collections.Generic;
using AppCore;
using AppCore.InputManagement;
using Game.GameManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Exploration.UI.Comic {
    public class ComicManager : MonoBehaviour {
        [SerializeField] private Image blackBackground;
        [SerializeField] private Transform[] pageParents;
        [SerializeField] private float fadeTime = 0.5f;
        [SerializeField] private bool waitForInput = true;
        [SerializeField] private float waitTime = 1.5f;
        [SerializeField] private float continueWaitTime = 0.5f;
        [SerializeField] private TextMeshProUGUI continuePopup;
        
        private ComicPage[] _comicPages;
        
        private bool _shouldContinue = false;
        private bool _hasContinued = false;
        private bool _isContinuePopupActive = false;
        
        private void OnEnable() {
            App.Get<InputManager>().OnDialogueContinue += OnContinue;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnDialogueContinue -= OnContinue;
        }

        private void OnContinue() {
            _shouldContinue = true;
            _hasContinued = true;
        }

        private void Awake() {
            _comicPages = new ComicPage[pageParents.Length];

            for (int i = 0; i < pageParents.Length; i++) {
                _comicPages[i] = new ComicPage(pageParents[i]);
                foreach (Image img in pageParents[i].GetComponentsInChildren<Image>()) {
                    _comicPages[i].ComicImage.Add(img);
                }
            }
            
            foreach (ComicPage comicPage in _comicPages) {
                foreach (Image image in comicPage.ComicImage) {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                }
            }
            
            blackBackground.gameObject.SetActive(false);
        }

        public void PlayComic() {
            blackBackground.gameObject.SetActive(true);
            blackBackground.color = new Color(blackBackground.color.r, blackBackground.color.g, blackBackground.color.b, 1);
            
            StartCoroutine(PlayComicRoutine());
            StartCoroutine(ShowContinuePopup());
        }

        private IEnumerator ShowContinuePopup() {
            yield return new WaitForSeconds(continueWaitTime);
            if (!_hasContinued) {
                StartCoroutine(FadeIn(continuePopup));
                _isContinuePopupActive = true;
            }
        }

        private IEnumerator PlayComicRoutine() {
            
            GameManager.GameEventType = GameEventType.Cutscene;
            foreach (ComicPage comicPage in _comicPages) {
                comicPage.Parent.gameObject.SetActive(true);
                foreach (Image image in comicPage.ComicImage) {
                    _shouldContinue = false;
                    yield return FadeIn(image);
                    
                    for (float t = 0; t < waitTime; t += Time.deltaTime) {
                        yield return null;
                        if (_shouldContinue) {
                            break;
                        }
                    }

                    if (waitForInput) {
                        yield return new WaitUntil(() => _shouldContinue);
                    }
                }
                yield return new WaitUntil(() => _shouldContinue);
                _shouldContinue = false;

                foreach (Image image in comicPage.ComicImage) {
                    StartCoroutine(FadeOut(image));
                }
                comicPage.Parent.gameObject.SetActive(false);
            }

            if (_isContinuePopupActive) {
                _isContinuePopupActive = false;
                StartCoroutine(FadeOut(continuePopup));
            }
            yield return FadeOut(blackBackground);
            GameManager.GameEventType = GameEventType.ExploreEnter;
            GameManager.GameEventType = GameEventType.Explore;
        }

        private IEnumerator FadeIn(TextMeshProUGUI text) {
            text.gameObject.SetActive(true);
            float t = 0;
            while (t < fadeTime) {
                t += Time.deltaTime;
                float a = Mathf.Lerp(0, 1, t / fadeTime);
                text.color = new Color(text.color.r, text.color.g, text.color.b, a);
                yield return null;
            }
        }

        private IEnumerator FadeIn(Image image) {
            float t = 0;
            while (t < fadeTime) {
                t += Time.deltaTime;
                float a = Mathf.Lerp(0, 1, t / fadeTime);
                image.color = new Color(image.color.r, image.color.g, image.color.b, a);
                yield return null;
            }
        }
        
        private IEnumerator FadeOut(TextMeshProUGUI text) {
            float t = 0;
            while (t < fadeTime) {
                t += Time.deltaTime;
                float a = Mathf.Lerp(1, 0, t / fadeTime);
                text.color = new Color(text.color.r, text.color.g, text.color.b, a);
                yield return null;
            }
            text.gameObject.SetActive(false);
        }
        
        private IEnumerator FadeOut(Image image) {
            float t = 0;
            while (t < fadeTime) {
                t += Time.deltaTime;
                float a = Mathf.Lerp(1, 0, t / fadeTime);
                image.color = new Color(image.color.r, image.color.g, image.color.b, a);
                yield return null;
            }
        }
    }

    public class ComicPage {
        public List<Image> ComicImage;
        public Transform Parent;
        
        public ComicPage(Transform parent) {
            ComicImage = new List<Image>();
            Parent = parent;
        }
    }
}