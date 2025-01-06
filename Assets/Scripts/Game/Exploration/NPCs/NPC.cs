using System;
using AppCore;
using AppCore.DialogueManagement;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.NPCs {
    public class NPC : MonoBehaviour, Interactable {
        [SerializeField] private GameObject interactablePopup;
        [SerializeField] private Dialogue[] dialogues;
        [SerializeField] private Dialogue reminderDialogue;
        
        private int _currentDialogueIndex = 0;
        
        Action _overCallback;

        private void Start() {
            interactablePopup.SetActive(false);
        }

        public void Interact(Action overCallback) {
            _overCallback = overCallback;
            if (_currentDialogueIndex >= dialogues.Length) {
                App.Get<DialogueManager>().StartDialogue(reminderDialogue, DialogueOver);
                return;
            }
            App.Get<DialogueManager>().StartDialogue(dialogues[_currentDialogueIndex], DialogueOver);
        }
        
        private void DialogueOver() {
            if (!(_currentDialogueIndex >= dialogues.Length)) {
                _currentDialogueIndex++;
            }
            _overCallback();
        }

        public void Hover() {
            interactablePopup.SetActive(true);
        }

        public void Unhover() {
            interactablePopup.SetActive(false);
        }
    }
}