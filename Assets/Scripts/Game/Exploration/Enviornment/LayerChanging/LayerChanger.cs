using System.Linq;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChanger : MonoBehaviour {
        [SerializeField] public float yOffset = 0f;
        internal SpriteRenderer[] SpriteRenderers;
        
        private LayerChangerData _layerChangerData;

        private void Start() {
            _layerChangerData = new LayerChangerData(this, yOffset);
            
            SpriteRenderer selfRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();

            SpriteRenderers = selfRenderer != null ? childRenderers.Concat(new[] { selfRenderer }).ToArray() : childRenderers;
            LayerChangingManager.RegisterLayerChanger(_layerChangerData);
        }

        private void OnDestroy() {
            LayerChangingManager.UnRegisterLayerChanger(_layerChangerData);
        }
    }
}