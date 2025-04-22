using System;
using AppCore;
using AppCore.DialogueManagement;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables {
    public class InteractableObject : MonoBehaviour, Interactable 
    {
        [SerializeField] public bool isInteractable = true;
        [SerializeField] protected Dialogue dialogue;
        private Action overCallback;
        
        public virtual void Interact(Action overCallback)
        {
            if (!isInteractable) {
                return;
            }
            this.overCallback = overCallback;
            InteractStarted();
            App.Get<DialogueManager>().StartDialogue(dialogue, EndInteraction);
        }

        public void SetIsInteractable(bool b) {
            isInteractable = b;
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