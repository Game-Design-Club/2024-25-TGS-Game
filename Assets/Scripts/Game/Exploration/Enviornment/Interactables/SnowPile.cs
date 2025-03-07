using System;
using AppCore;
using AppCore.DataManagement;
using AppCore.DialogueManagement;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables
{
    public class SnowPile : MonoBehaviour, Interactable {
        [SerializeField] private Dialogue preStick;
        [SerializeField] private Dialogue postStick;
        [SerializeField] private ParticleSystem interactableParticleSystem;
        [SerializeField] private ParticleSystem attackableParticleSystem;
        
        public void Interact(Action overCallback)
        {
            bool hasStick = App.Get<DataManager>().HasStick;
            Dialogue dialogue = !hasStick ? preStick : postStick;
            App.Get<DialogueManager>().StartDialogue(dialogue, overCallback);
        }

        public void Hover()
        {
            bool hasStick = App.Get<DataManager>().HasStick;
            ParticleSystem tempParticleSystem = !hasStick ? interactableParticleSystem : attackableParticleSystem;
            tempParticleSystem.Play();
        }

        public void Unhover()
        {
            bool hasStick = App.Get<DataManager>().HasStick;
            ParticleSystem tempParticleSystem = !hasStick ? interactableParticleSystem : attackableParticleSystem;
            tempParticleSystem.Stop();
        }
    }
}