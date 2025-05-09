using System;
using AppCore;
using AppCore.DataManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables {
    public class GrowlPickup : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            App.Get<DataManager>().SetFlag(BoolFlags.HasGrowl, true);
        }
    }
}