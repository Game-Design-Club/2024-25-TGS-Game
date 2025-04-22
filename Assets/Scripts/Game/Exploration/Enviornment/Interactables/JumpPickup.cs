using System;
using AppCore;
using AppCore.DataManagement;
using AppCore.DialogueManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables
{
    public class JumpPickup : InteractableObject
    {
        [SerializeField] private GameObject jumpObject;
        [SerializeField] private PickupParticle pickupParticleObject;
        private Action overCallback;


        public override void InteractionEnded()
        {
            App.Get<DataManager>().SetFlag(BoolFlags.HasJump, true);
            pickupParticleObject.PlayAndDetach();
            Destroy(jumpObject);
        }
    }
}