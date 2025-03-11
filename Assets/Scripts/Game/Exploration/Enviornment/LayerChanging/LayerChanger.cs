using System.Linq;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChanger : MonoBehaviour {
        internal SpriteRenderer[] SpriteRenderers;

        private void Start() {
            SpriteRenderer selfRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();

            SpriteRenderers = selfRenderer != null ? childRenderers.Concat(new[] { selfRenderer }).ToArray() : childRenderers;
            Debug.Log(SpriteRenderers.Length);
            LayerChangingManager.RegisterLayerChanger(this);
        }

        private void OnDestroy() {
            LayerChangingManager.UnRegisterLayerChanger(this);
        }
    }
}