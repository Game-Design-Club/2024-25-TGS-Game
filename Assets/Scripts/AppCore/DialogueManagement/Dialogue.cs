using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace AppCore.DialogueManagement {
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject, IEnumerable<DialogueChunk> {
        [SerializeField] internal DialogueChunk[] dialogueChunks;
        
        private int _currentChunkIndex;
        
        public IEnumerator<DialogueChunk> GetEnumerator() {
            for (_currentChunkIndex = 0; _currentChunkIndex < dialogueChunks.Length; _currentChunkIndex++) {
                yield return dialogueChunks[_currentChunkIndex];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        
        public bool IsEmpty => dialogueChunks.Length == 0;

        public void StartDialogue(Action callback) {
            App.Get<DialogueManager>().StartDialogue(this, callback);
        }
    }
}