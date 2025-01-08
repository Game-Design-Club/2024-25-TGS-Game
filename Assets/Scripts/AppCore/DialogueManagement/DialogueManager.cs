using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppCore.InputManagement;
using Game.GameManagement;
using NUnit.Framework;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AppCore.DialogueManagement {
    public class DialogueManager : AppModule {
        [FormerlySerializedAs("effectsDat")]
        [FormerlySerializedAs("textEffectsData")]
        [Header("Settings")]
        [SerializeField] private TextEffectsData effectsData;
        [SerializeField] private float scrollSpeed = 1;
        [FormerlySerializedAs("_punctuationPauseTime")]
        [Header("Auto pause")]
        [SerializeField] private CharacterAutoPause[] autoPauseCharacters;
        [Header("References")]
        [SerializeField] private GameObject dialogueBox;
        
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Image characterSpriteRenderer;
        
        private Dialogue _currentDialogue;
        
        private bool _shouldContinue;
        
        private bool _isScrollingDialogue;
        
        private List<(int, string)> _wobbleCharacters = new();

        private Animator _animator;
        
        // Unity functions
        private void Awake() {
            dialogueBox.SetActive(false);
            _animator = GetComponent<Animator>();
        }

        private void OnEnable() {
            App.Get<InputManager>().OnDialogueContinue += OnContinue;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnDialogueContinue -= OnContinue;
        }
        
        private void OnContinue() {
            _shouldContinue = true;
        }

        public void StartDialogue(Dialogue dialogue, Action callback) {
            if (_currentDialogue != null) {
                Debug.LogWarning(_currentDialogue == dialogue
                    ? "Tried to play cutscene while already playing that dialogue"
                    : "Tried to play a cutscene while playing another dialogue");
            }
            
            _currentDialogue = dialogue;
            StartCoroutine(PlayDialogue(callback));
        }
        
        // Private functions
        private IEnumerator PlayDialogue(Action callback) {
            GameManager.DialogueStart();
            
            dialogueBox.SetActive(true);
            
            _animator.SetBool(Constants.Animator.DialogueBox.IsOpen, true);
            
            _wobbleCharacters.Clear();
            foreach (DialogueChunk currentChunk in _currentDialogue) {
                SetupChunk(currentChunk);
                
                yield return StartCoroutine(TypeOutChunk(currentChunk));
                
                _wobbleCharacters.Clear();
            }
            
            _currentDialogue = null;
            GameManager.DialogueEnd();
            callback.Invoke();
            
            _animator.SetBool(Constants.Animator.DialogueBox.IsOpen, false);
            yield return new WaitForSeconds(0.5f);
            dialogueBox.SetActive(false);
        }
        
        private void SetupChunk(DialogueChunk chunk) {
            dialogueText.text = "";
            characterNameText.text = chunk.character.name;
            characterSpriteRenderer.sprite = chunk.character.sprite;
        }

        private IEnumerator TypeOutChunk(DialogueChunk chunk) {
                yield return null; // Otherwise dialogue will be skipped because start dialogue button is the same as continue button
                _shouldContinue = false;
                
                var parsedChunk = chunk.ParseEffects();
                for (int i = 0; i < parsedChunk.Count; i++) {
                    if (_shouldContinue) {
                        _shouldContinue = false;
                        for (int j = i; j < parsedChunk.Count; j++) {
                            yield return StartCoroutine(AppendCharacter(parsedChunk, j, true));
                        }
                        break;
                    }

                    yield return StartCoroutine(AppendCharacter(parsedChunk, i));
                    yield return new WaitForSecondsRealtime(1 / scrollSpeed);
                }
                _shouldContinue = false;

                yield return new WaitUntil(() => _shouldContinue);

                _shouldContinue = false;
        }

        private IEnumerator AppendCharacter(List<(char character, List<TextEffect> effects)> parsedCharacters, int index, bool ignorePause = false) {
            var (character, effects) = parsedCharacters[index];
            foreach (var effect in effects) {
                yield return StartCoroutine(ApplyEffect(effect, index, ignorePause));
            }
            dialogueText.text += character;
            
            if (ignorePause) yield break;
            foreach (var autoPauseCharacter in autoPauseCharacters) {
                if (autoPauseCharacter.character == character) {
                    yield return new WaitForSecondsRealtime(autoPauseCharacter.pauseDuration);
                }
            }
        }
        
        private IEnumerator ApplyEffect(TextEffect effect, int index, bool ignorePause) {
            // If color, add <color=data> string to text so textmeshpro can parse it
            // If bold, add <b> tag to text so textmeshpro can parse it
            // If wobble, add character index to wobble list to be used in Update
            if (effect.Type == TextEffectType.Color) {
                dialogueText.text += $"<{effect.Data}>";
            } else if (effect.Type == TextEffectType.Bold) {
                dialogueText.text += "<b>";
            } else if (effect.Type == TextEffectType.Wobble) {
                _wobbleCharacters.Add((index, (string)effect.Data));
            } else if (effect.Type == TextEffectType.Pause) {
                if (!ignorePause) {
                    yield return new WaitForSecondsRealtime((float)effect.Data);
                }
            }
        }

        private void Update() {
            ApplyWobble();
        }

        private void ApplyWobble() { 
            // Force an update so we can manipulate the mesh
            dialogueText.ForceMeshUpdate();

            TMP_TextInfo textInfo = dialogueText.textInfo;
            float time = Time.unscaledTime;

            // For each stored wobble index
            for (int i = 0; i < _wobbleCharacters.Count; i++) {
                var (charIndex, wobbleName) = _wobbleCharacters[i];
                WobbleData wobble = Array.Find(effectsData.wobbleData, w => w.name == wobbleName);
                
                if (charIndex >= textInfo.characterCount) 
                    continue;
            
                TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
            
                if (!charInfo.isVisible) 
                    continue;

                Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                float charXNoise = Random.Range(-wobble.xNoise, wobble.xNoise);
                float charYNoise = Random.Range(-wobble.yNoise, wobble.yNoise);


                // Each character is made of 4 verts
                for (int j = 0; j < 4; j++) {
                    Vector3 orig = verts[charInfo.vertexIndex + j];

                    float xOffset = 0;
                    float yOffset = 0;
                    xOffset += (float)Math.Sin(
                        time * wobble.xSpeed + 
                        charIndex * wobble.xOffset +
                        j * wobble.xVertMultiplier
                        ) * wobble.xAmplitude;


                    yOffset += (float)Math.Sin(
                        time * wobble.ySpeed + 
                        charIndex * wobble.yOffset +
                        j * wobble.yVertMultiplier
                        ) * wobble.yAmplitude;
                    
                    xOffset += charXNoise;
                    yOffset += charYNoise;

                    verts[charInfo.vertexIndex + j] = orig + new Vector3(xOffset, yOffset, 0);
                }
            }

            // Push changes into the mesh
            for (int i = 0; i < textInfo.meshInfo.Length; i++) {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                dialogueText.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }
}