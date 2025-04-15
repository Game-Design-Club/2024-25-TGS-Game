using System;
using System.Collections;
using System.Collections.Generic;
using AppCore.InputManagement;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AppCore.DialogueManagement {
    public class DialogueManager : AppModule {
        [Header("Settings")]
        [SerializeField] internal TextEffectsData effectsData;
        [SerializeField] private float scrollSpeed = 1f;

        [Header("Auto pause")]
        [SerializeField] private CharacterAutoPause[] autoPauseCharacters;

        [Header("References")]
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Image characterSpriteRenderer;

        private Dialogue _currentDialogue;
        private bool _shouldContinue;

        private int _typedIndex; // How many characters are currently shown

        private Dictionary<int, float> _animatingCharacters = new();

        private Dictionary<int, string> _wobbleCharacters = new();

        private Animator _animator;
        private Action _onDialogueComplete;
        
        public event Action OnDialogueStart;
        public event Action OnDialogueEnd;
        
        private bool _isPlayingDialogue => _currentDialogue != null;

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

        public void StartDialogue(Dialogue dialogue, Action callback)
        {
            if (_currentDialogue != null) {
                Debug.LogWarning(_currentDialogue == dialogue
                    ? "Tried to play dialogue while already playing that dialogue"
                    : "Tried to play dialogue while playing another dialogue");
            }

            _currentDialogue = dialogue;
            _onDialogueComplete = callback;
            StartCoroutine(PlayDialogue());
        }

        private IEnumerator PlayDialogue() {
            OnDialogueStart?.Invoke();
            dialogueBox.SetActive(true);
            _animator.SetBool(AnimationConstants.DialogueBox.IsOpen, true);

            // For each chunk in the dialogue:
            foreach (DialogueChunk currentChunk in _currentDialogue) {
                SetupChunk(currentChunk);
                // Wait for this chunk to finish
                yield return StartCoroutine(TypeOutChunk(currentChunk));
            }

            // Clean up
            _currentDialogue = null;
            OnDialogueEnd?.Invoke();
            _onDialogueComplete?.Invoke();
            _animator.SetBool(AnimationConstants.DialogueBox.IsOpen, false);
        }

        private void SetupChunk(DialogueChunk chunk) {
            // Clear out old data
            _typedIndex = 0;
            _wobbleCharacters.Clear();
            _animatingCharacters.Clear();

            // Set name and sprite
            if (chunk.character != null) {
                characterNameText.text = chunk.character.name;
                characterSpriteRenderer.sprite = chunk.character.sprite;
            } else {
                characterNameText.text = "";
                characterSpriteRenderer.sprite = null;
            }

            // Parse chunk for effects
            var parsedChunk = chunk.ParseEffects(this); 
            // Build a single string with all the text and tags
            string fullText = BuildFullString(parsedChunk);

            // Then set the entire text at once
            dialogueText.text = fullText;

            dialogueText.ForceMeshUpdate();
        }

        private IEnumerator TypeOutChunk(DialogueChunk chunk) {
            yield return null;
            _shouldContinue = false;

            var parsedChunk = chunk.ParseEffects(this);

            for (int i = 0; i < parsedChunk.Count; i++) {
                // If user pressed skip, reveal the rest instantly
                if (_shouldContinue) {
                    RevealAllRemainingCharacters(parsedChunk);
                    break;
                }

                // "Reveal" this next character
                _typedIndex = i + 1; // typedIndex is "how many chars are shown"
                StartAnimationForCharacter(i, parsedChunk[i].effects);

                // AutoPause if this character triggers it
                char c = parsedChunk[i].character;
                yield return StartCoroutine(HandleAutoPause(c));

                // If pressed skip while in a pause, break out
                if (_shouldContinue) {
                    RevealAllRemainingCharacters(parsedChunk);
                    break;
                }

                // Wait for normal "type speed" delay
                float waitTime = 1f / scrollSpeed;
                float timer = 0f;
                while (timer < waitTime) {
                    if (_shouldContinue) {
                        RevealAllRemainingCharacters(parsedChunk);
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            // After finishing the chunk, wait for the user to press continue again
            _shouldContinue = false;
            yield return new WaitUntil(() => _shouldContinue);
            _shouldContinue = false;
        }

        private void RevealAllRemainingCharacters(List<(char character, List<TextEffect> effects)> parsedChunk) {
            _typedIndex = parsedChunk.Count;
            // Start animate all those not started yet
            for (int i = 0; i < parsedChunk.Count; i++) {
                if (!_animatingCharacters.ContainsKey(i)) {
                    StartAnimationForCharacter(i, parsedChunk[i].effects, skip:true);
                }
            }
        }

        /// Start the appear/wobble for the character at index, storing data in dictionaries.
        /// If skip==true, we mark the character as "fully done" so it doesn't float down.
        private void StartAnimationForCharacter(int index, List<TextEffect> effects, bool skip=false) {
            // Mark this character as newly revealed
            _animatingCharacters[index] = skip ? effectsData.appearDuration : 0f;

            // Also see if there's a wobble effect
            foreach (var eff in effects) {
                if (eff.Type == TextEffectType.Wobble) {
                    _wobbleCharacters[index] = (string)eff.Data;
                }
            }
        }

        /// <summary>
        /// Checks if the typed character is in autoPauseCharacters; if so, does a short wait.
        /// </summary>
        private IEnumerator HandleAutoPause(char c) {
            foreach (var autoPauseCharacter in autoPauseCharacters) {
                if (autoPauseCharacter.character == c) {
                    float timer = 0f;
                    while (timer < autoPauseCharacter.pauseDuration) {
                        if (_shouldContinue) yield break;
                        timer += Time.deltaTime;
                        yield return null;
                    }
                }
            }
        }

        private string BuildFullString(List<(char character, List<TextEffect> effects)> parsed) {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (var (character, effects) in parsed) {
                foreach (var e in effects) {
                    switch (e.Type) {
                        case TextEffectType.Color:
                            sb.Append($"<{e.Data}>");
                            break;
                        case TextEffectType.Bold:
                            sb.Append("<b>");
                            break;
                        case TextEffectType.Italic:
                            sb.Append("<i>");
                            break;
                    }
                }

                sb.Append(character);
            }

            return sb.ToString();
        }

        /// LateUpdate: we now set alpha/offset for each character based on whether it’s revealed or not, and any effects.
        private void LateUpdate() {
            if (!_isPlayingDialogue) return;
            dialogueText.ForceMeshUpdate();
            ApplyEffectsToText();
        }

        private void ApplyEffectsToText() {
            TMP_TextInfo textInfo = dialogueText.textInfo;
            float time = Time.unscaledTime;

            for (int i = 0; i < textInfo.characterCount; i++) {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                // 4 vertices
                int matIndex = charInfo.materialReferenceIndex;
                int vertIndex = charInfo.vertexIndex;
                Vector3[] verts = textInfo.meshInfo[matIndex].vertices;
                Color32[] colors = textInfo.meshInfo[matIndex].colors32;

                // If this character is not yet revealed (i >= typedIndex), set alpha=0
                // and skip any appear/wobble logic.
                if (i >= _typedIndex) {
                    for (int j = 0; j < 4; j++) {
                        Color32 origColor = colors[vertIndex + j];
                        colors[vertIndex + j] = new Color32(origColor.r, origColor.g, origColor.b, 0);
                    }
                    continue; 
                }

                // The character is revealed, so do appear/wobble
                float animTime = 0f;
                if (_animatingCharacters.ContainsKey(i)) {
                    // Increase that character's "time since reveal"
                    _animatingCharacters[i] += Time.deltaTime;
                    animTime = _animatingCharacters[i];
                    if (animTime > effectsData.appearDuration) {
                        animTime = effectsData.appearDuration;
                    }
                }

                // "t" = how far along we are in the appear animation [0..1]
                float t = Mathf.Clamp01(animTime / effectsData.appearDuration);
                float yOffset = Mathf.Lerp(effectsData.appearHeight, 0f, t);
                float alpha = t; // fade from 0 -> 1

                // Wobble?
                float[] xVertOffsets = new float[] { 0,0,0,0};
                float[] yVertOffsets = new float[] { 0,0,0,0};
                if (_wobbleCharacters.ContainsKey(i)) {
                    var wobble = Array.Find(effectsData.wobbleData,
                        w => w.name == _wobbleCharacters[i]);
                    if (wobble != null) {
                        
                        float xJitter = Random.Range(-wobble.xNoise, wobble.xNoise);
                        float yJitter = Random.Range(-wobble.yNoise, wobble.yNoise);

                        // basic wave
                        for (int j = 0; j < 4; j++) {
                            xVertOffsets[j] +=
                                Mathf.Sin(time * wobble.xSpeed +
                                          i * wobble.xOffset +
                                          j * wobble.xVertMultiplier) *
                                wobble.xAmplitude + xJitter;
                            yVertOffsets[j] +=
                                Mathf.Sin(time * wobble.ySpeed + i * wobble.yOffset + j * wobble.yVertMultiplier) *
                                wobble.yAmplitude + yJitter;
                        }
                        
                        // random jitter
                    }
                }

                // Apply final offset + alpha
                for (int j = 0; j < 4; j++) {
                    Vector3 orig = verts[vertIndex + j];
                    
                    float xOffset = xVertOffsets[j];
                    float yAnimOffset = yOffset + yVertOffsets[j];

                    // Move the vertex
                    verts[vertIndex + j] = orig + new Vector3(xOffset, yAnimOffset, 0f);

                    // Set alpha
                    Color32 origColor = colors[vertIndex + j];
                    byte a = (byte)Mathf.Clamp(Mathf.RoundToInt(255f * alpha), 0, 255);
                    colors[vertIndex + j] = new Color32(origColor.r, origColor.g, origColor.b, a);
                }
            }

            // Push changes to the mesh
            for (int i = 0; i < textInfo.meshInfo.Length; i++) {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                meshInfo.mesh.colors32 = meshInfo.colors32;
                dialogueText.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }
}