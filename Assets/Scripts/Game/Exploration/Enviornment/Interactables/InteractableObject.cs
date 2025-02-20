using System;
using AppCore;
using AppCore.DialogueManagement;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables {
    public class InteractableObject : MonoBehaviour, Interactable 
    {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private string dialogueThing;
        
        public void Interact(Action overCallback) {
            App.Get<DialogueManager>().StartDialogue(dialogue, overCallback);
        }

        public void Hover() {
        }

        public void Unhover() {
        }
    }
}