using System;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChanger : MonoBehaviour {
        internal SpriteRenderer[] SpriteRenderers;
        private void Start() {
            SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            LayerChangingManager.RegisterLayerChanger(this);
        }

        private void OnDestroy() {
            LayerChangingManager.UnRegisterLayerChanger(this);
        }
    }
}
