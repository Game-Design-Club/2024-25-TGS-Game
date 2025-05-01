using System;
using System.Collections;
using AppCore;
using AppCore.InputManagement;
using MainMenu.Management;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace MainMenu.Credits {
    public class CreditsScroller : MonoBehaviour {
        // Manages the creation and scrolling of the credits, based on the CreditsAsset
        // Automatically scrolls the credits up the screen, and displays a thank you message when the credits are done
        
        [SerializeField] private MainMenuManager mainMenuManager;
        [SerializeField] private CreditsAsset creditsAsset;
        [SerializeField] private GameObject sectionTitlePrefab;
        [SerializeField] private GameObject personNamePrefab;
        [SerializeField] private GameObject creditsParentObject;
        [SerializeField] private float scrollSpeed = 1f;
        [SerializeField] private int characterCountWrap = 15;
        [SerializeField] private float spaceBetweenSections = 200f;
        [SerializeField] private GameObject thankYouObject;
        [SerializeField] private float delayBeforeThing = .5f;

        private float _endY;
        private bool _scrolling = true;

        // Unity functions
        private void OnEnable() {
            App.Get<InputManager>().OnUICancel += OnCancelPressed;
        }

        private void OnDisable() {
            App.Get<InputManager>().OnUICancel -= OnCancelPressed;
        }

        private void Start() {
            thankYouObject.SetActive(false);
            creditsParentObject.SetActive(false);
        }

        private void Update() {
            if (!_scrolling) return;
            Debug.Log("Scrolling");
            creditsParentObject.transform.position += Vector3.up * (scrollSpeed * Time.unscaledDeltaTime);
            if (creditsParentObject.transform.localPosition.y > _endY) {
                _scrolling = false;
                ShowThankYou();
            }
        }

        // Private functions
        private void SetupCredits() {
            float currentY = 0f;
            float creditsHeight = 0f;
            
            foreach (CreditsSection section in creditsAsset.creditsSections) {
                GameObject sectionObject = Instantiate(sectionTitlePrefab, creditsParentObject.transform);
                sectionObject.GetComponent<TextMeshProUGUI>().SetText(section.title);
                RectTransform sectionTransform = sectionObject.GetComponent<RectTransform>();
                sectionTransform.anchoredPosition = new Vector2(sectionTransform.anchoredPosition.x, currentY);
                
                float sectionHeight = sectionObject.GetComponent<RectTransform>().rect.height;
                currentY -= sectionHeight;
                creditsHeight += sectionHeight;
                
                foreach (String personName in section.names) {
                    GameObject nameObject = Instantiate(personNamePrefab, creditsParentObject.transform);
                    nameObject.GetComponent<TextMeshProUGUI>().SetText(personName);
                    RectTransform nameTransform = nameObject.GetComponent<RectTransform>();
                    nameTransform.anchoredPosition = new Vector2(nameTransform.anchoredPosition.x, currentY);
                    
                    float nameHeight = nameObject.GetComponent<RectTransform>().rect.height;
                    currentY -= nameHeight;
                    creditsHeight += nameHeight;
                }
                if (section.names.Length <= 1 && section.title.Length > characterCountWrap) {
                    currentY -= spaceBetweenSections;
                    creditsHeight += spaceBetweenSections;
                }
                currentY -= spaceBetweenSections;
                creditsHeight += spaceBetweenSections;
            }
            
            _endY = creditsHeight;
        }
        
        private void OnCancelPressed() {
            StartCoroutine(StopCreditsCoroutine());
        }

        private IEnumerator StopCreditsCoroutine() {
            mainMenuManager.UnCreditsButton();
            yield return new WaitForSeconds(delayBeforeThing);
            StopCredits();
        }

        private void ShowThankYou() {
            creditsParentObject.SetActive(false);
            thankYouObject.SetActive(true);
        }
        
        public void StartCredits() {
            creditsParentObject.SetActive(true);
            thankYouObject.SetActive(false);
            _scrolling = true;
            creditsParentObject.transform.localPosition = new Vector3(0, 0, 0);
            SetupCredits();
        }

        public void StopCredits() {
            creditsParentObject.SetActive(false);
            thankYouObject.SetActive(false);
            _scrolling = false;
        }
    }
}