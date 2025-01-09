using System;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChanger : MonoBehaviour {
        [SerializeField] internal SpriteRenderer spriteRenderer;

        private void Start() {
            LayerChangingManager.RegisterLayerChanger(this);
        }
    }
}
