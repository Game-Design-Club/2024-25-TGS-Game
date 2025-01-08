using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AppCore.DialogueManagement {
    [Serializable]
    public class DialogueChunk {
        [SerializeField] public DialogueCharacter character;
        [TextArea(3, 10)]
        [SerializeField] private string text;
        
        private List<(char character, List<TextEffect> effects)> _parsedText;
        private bool _parsed = false;

        private static readonly TextEffectType[] ExpirableEffects = {
            TextEffectType.Color, TextEffectType.Bold, TextEffectType.Italic, TextEffectType.Pause
        };
        
        

        private List<TextEffect> _activeEffects = new();

        public List<(char character, List<TextEffect> effects)> ParseEffects(DialogueManager manager) {
            if (_parsedText != null) return _parsedText;
            _parsedText = new();
            string text = this.text;

            for (int i = 0; i < text.Length; i++) {
                if (text[i] == '<') {
                    int endTagIndex = text.IndexOf('>', i);
                    if (endTagIndex != -1) {
                        string tag = text.Substring(i + 1, endTagIndex - i - 1);
                        GetTagEffect(tag);
                        i = endTagIndex;
                        continue;
                    }
                }
                
                _parsedText.Add((text[i], new List<TextEffect>(_activeEffects)));
                
                RemoveExpiredEffects();
            }
            
            _parsed = true;
            return _parsedText;
            
            void GetTagEffect(string tag) {
                if (tag.Contains("color")) {
                    if (EffectsContain(TextEffectType.Color)) {
                        RemoveEffect(TextEffectType.Color);
                    }
                    AddEffect(TextEffectType.Color, tag);
                    return;
                }
                if (tag == "/color") {
                    if (EffectsContain(TextEffectType.Color)) {
                        RemoveEffect(TextEffectType.Color);
                    }
                    AddEffect(TextEffectType.Color, "color=#000000");
                    return;
                }
                
                if (tag == "bold") {
                    if (EffectsContain(TextEffectType.Bold)) {
                        return;
                    }
                    AddEffect(TextEffectType.Bold);
                    return;
                }
                if (tag == "/bold") {
                    if (EffectsContain(TextEffectType.Bold)) {
                        RemoveEffect(TextEffectType.Bold);
                    }
                    AddEffect(TextEffectType.Bold);
                    return;
                }
                
                if (tag.Contains("wobble")) {
                    if (EffectsContain(TextEffectType.Wobble)) {
                        RemoveEffect(TextEffectType.Wobble);
                    }
                    string name = tag.Substring(tag.IndexOf('=') + 1); // example: wobble=0, name is "0"
                    AddEffect(TextEffectType.Wobble, name);
                    return;
                }
                if (tag == "/wobble") {
                    if (EffectsContain(TextEffectType.Wobble)) {
                        RemoveEffect(TextEffectType.Wobble);
                    }
                    return;
                }
                
                if (tag == "italic") {
                    if (EffectsContain(TextEffectType.Italic)) {
                        return;
                    }
                    AddEffect(TextEffectType.Italic);
                    return;
                }
                if (tag == "/italic") {
                    if (EffectsContain(TextEffectType.Italic)) {
                        RemoveEffect(TextEffectType.Italic);
                    }
                    AddEffect(TextEffectType.Italic);
                    return;
                }

                if (tag.Contains("pause")) {
                    if (EffectsContain(TextEffectType.Pause)) {
                        RemoveEffect(TextEffectType.Pause);
                    }
                    AddEffect(TextEffectType.Pause, float.Parse(tag.Substring(tag.IndexOf('=') + 1)));
                    return;
                }
                
                if (manager.effectsData.wobbleData.Any(data => data.name == tag)) {
                    if (EffectsContain(TextEffectType.Wobble)) {
                        RemoveEffect(TextEffectType.Wobble);
                    }
                    AddEffect(TextEffectType.Wobble, tag);
                    return;
                }
                // if / wobble
                if (tag[0] == '/' && manager.effectsData.wobbleData.Any(data => data.name == tag.Substring(1))) {
                    if (EffectsContain(TextEffectType.Wobble)) {
                        RemoveEffect(TextEffectType.Wobble);
                    }
                    return;
                }
                
                Debug.LogWarning("Unknown tag: " + tag);
            }

        }
        
        private void RemoveExpiredEffects() {
            // Expired effects are ones that don't work if effects is added to multiple characters
            // In textmeshpro, that'll be like colors and bold
            // For wobble on the other hand, it's fine to have it on multiple characters
            
            _activeEffects.RemoveAll(effect => ExpirableEffects.Contains(effect.Type));
        }
        
        private void AddEffect(TextEffectType type, object data = null) {
            _activeEffects.Add(new TextEffect(type, data));
        }
        
        private void RemoveEffect(TextEffectType type) {
            _activeEffects.RemoveAll(effect => effect.Type == type);
        }
        
        private bool EffectsContain(TextEffectType type) {
            return _activeEffects.Any(effect => effect.Type == type);
        }
    }
}