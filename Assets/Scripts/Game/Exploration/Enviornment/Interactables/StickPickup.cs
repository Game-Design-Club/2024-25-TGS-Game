using System;
using AppCore;
using AppCore.DataManagement;
using AppCore.DialogueManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables
{
    public class StickPickup : InteractableObject
    {
        [SerializeField] private GameObject stickObject;
        [SerializeField] private PickupParticle pickupParticleObject;
        private Action overCallback;


        public override void InteractionEnded()
        {
            EndInteraction();
            App.Get<DataManager>().ObtainedStick();
            pickupParticleObject.PlayAndDetach();
            Destroy(stickObject);
        }
    }
}
