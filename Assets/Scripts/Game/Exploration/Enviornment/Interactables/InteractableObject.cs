using System;
using AppCore;
using AppCore.DialogueManagement;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables {
    public class InteractableObject : MonoBehaviour, Interactable 
    {
        [SerializeField] protected Dialogue dialogue;
        private Action overCallback;
        
        public virtual void Interact(Action overCallback)
        {
            this.overCallback = overCallback;
            InteractStarted();
            App.Get<DialogueManager>().StartDialogue(dialogue, EndInteraction);
        }

        public void Hover() {
            
        }

        public void Unhover() {
        }

        public virtual void InteractStarted(){}

        public virtual void InteractionEnded()
        {
        }

        public void EndInteraction()
        {
            InteractionEnded();
            overCallback();
        }

    }
}