using System;
using AppCore;
using AppCore.DataManagement;
using AppCore.DialogueManagement;
using Game.Exploration.Child;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables
{
    public class SnowPile : MonoBehaviour, Interactable {
        [SerializeField] private Dialogue preStick;
        [SerializeField] private Dialogue postStick;
        
        public void Interact(Action overCallback)
        {
            bool hasStick = App.Get<DataManager>().GetFlag(BoolFlags.HasStick);
            Dialogue dialogue = !hasStick ? preStick : postStick;
            App.Get<DialogueManager>().StartDialogue(dialogue, overCallback);
        }

        public void Hover()
        {
            bool hasStick = App.Get<DataManager>().GetFlag(BoolFlags.HasStick);
        }

        public void Unhover()
        {
            bool hasStick = App.Get<DataManager>().GetFlag(BoolFlags.HasStick);
        }
    }
}