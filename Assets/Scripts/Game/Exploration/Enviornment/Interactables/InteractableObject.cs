using System;
using AppCore;
using AppCore.DialogueManagement;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables {
    public class InteractableObject : MonoBehaviour, Interactable 
    {
        [SerializeField] internal Dialogue dialogue;
        [SerializeField] private ParticleSystem interactableParticleSystem;
        private Action overCallback;
        
        public virtual void Interact(Action overCallback)
        {
            this.overCallback = overCallback;
            App.Get<DialogueManager>().StartDialogue(dialogue, InteractionEnded);
        }

        public void EndInteraction()
        {
            overCallback();
        }

        public virtual void InteractionEnded()
        {
            EndInteraction();
        }

        public void Hover() {
            interactableParticleSystem.Play();
        }

        public void Unhover() {
            interactableParticleSystem.Stop();
        }

    }
}