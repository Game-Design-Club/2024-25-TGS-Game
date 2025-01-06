using System;
using System.Collections;
using AppCore.InputManagement;
using Game.GameManagement;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace AppCore.DialogueManagement {
    public class DialogueManager : AppModule {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private GameObject leftAligned;
        [SerializeField] private GameObject rightAligned;
        
        [SerializeField] private TextMeshProUGUI[] dialogueText;
        [SerializeField] private TextMeshProUGUI[] characterNameText;
        [SerializeField] private Image[] characterSpriteRenderer;

        [SerializeField] private float scrollSpeed = 1;
        [SerializeField] private bool skipOnInput = true;
        // [SerializeField] private SoundPackage continueSound;
        
        private Dialogue _currentDialogue;
        
        private bool _shouldContinue;
        
        private bool _isScrollingDialogue;
        
        // Unity functions
        private void Awake() {
            dialogueBox.SetActive(false);
        }

        private void OnEnable() {
            App.Get<InputManager>().OnDialogueContinue += OnContinue;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnDialogueContinue -= OnContinue;
        }

        // Private functions
        private IEnumerator PlayDialogue(Action callback) {
            GameManager.DialogueStart();
            
            dialogueBox.SetActive(true);
            _shouldContinue = false;
            
            foreach (DialogueChunk currentChunk in _currentDialogue) {
                PlayDialogueChunk(currentChunk);

                if (skipOnInput) {
                    int totalCharacters = currentChunk.text.Length;
                    float currentCharacters = 0;
                    while (!_shouldContinue && currentCharacters < totalCharacters) {
                        if (currentCharacters < totalCharacters) {
                            currentCharacters += scrollSpeed * Time.unscaledDeltaTime;
                        }

                        int charactersToShow = Mathf.Clamp((int)currentCharacters, 0, totalCharacters);
                        UpdateText(currentChunk.text.Substring(0, charactersToShow));
                        yield return null;
                    }
                }
                _shouldContinue = false;
                
                UpdateText(currentChunk.text);
                
                yield return new WaitUntil(() => _shouldContinue);
                
                _shouldContinue = false;
            }

            _currentDialogue = null;
            dialogueBox.SetActive(false);
            
            GameManager.DialogueEnd();
            
            callback.Invoke();
        }

        private void OnContinue() {
            _shouldContinue = true;
        }
        
        private void PlayDialogueChunk(DialogueChunk chunk) {
            Array.ForEach(characterNameText, textGUI => textGUI.text = "");
            Array.ForEach(dialogueText, textGUI => {
                textGUI.text = chunk.text;
                textGUI.color = chunk.character.textColor;
            });
            Array.ForEach(characterSpriteRenderer, image => image.sprite = chunk.character.characterSprite);
            if (chunk.character.textAlignment == TextAlignment.Left) {
                leftAligned.SetActive(true);
                rightAligned.SetActive(false);
            } else {
                leftAligned.SetActive(false);
                rightAligned.SetActive(true);
            }
        }
        
        private void UpdateText(string text) {
            Array.ForEach(dialogueText, textGUI => textGUI.text = text);
        }
        
        // Public functions
        public void StartDialogue(Dialogue dialogue, Action callback) {
            if (_currentDialogue != null) {
                Debug.LogWarning(_currentDialogue == dialogue
                    ? "Tried to play cutscene while already playing that dialogue"
                    : "Tried to play a cutscene while playing another dialogue");
            }
            
            _currentDialogue = dialogue;
            StartCoroutine(PlayDialogue(callback));
        }
    }
}