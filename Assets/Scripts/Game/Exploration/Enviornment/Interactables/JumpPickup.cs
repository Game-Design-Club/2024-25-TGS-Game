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


        public override void InteractionEnded()
        {
            pickupParticleObject.PlayAndDetach();
            Destroy(jumpObject);
        }
    }
}