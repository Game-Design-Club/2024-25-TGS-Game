using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChangingManager : MonoBehaviour {
        [SerializeField] private Transform child;
        private readonly List<LayerChangerData> _layerChangers = new();

        private static LayerChangingManager _instance;
        private float _childY;

        private void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public static void RegisterLayerChanger(LayerChangerData changerData, float yOffset = 0) {
            if (_instance == null) return;

            _instance._layerChangers.Add(changerData);
            LayerChanger changer = changerData.Changer;
            foreach (var sr in changer.SpriteRenderers) {
                sr.sortingOrder = Mathf.RoundToInt(changer.transform.position.y * 100f) * -1;
            }
        }

        private void Update() {
            _childY = child.position.y;
            
            int changerCount = _layerChangers.Count;
            for (int i = 0; i < changerCount; i++) {
                LayerChangerData changerData = _layerChangers[i];
                LayerChanger changer = changerData.Changer;
                bool childIsBehind = (_childY - (changer.transform.position.y + changerData.YOffset)) < 0;
                string targetLayer = childIsBehind ? Layers.ChildGameplay : Layers.ChildGameplayFront;

                var spriteRenderers = changer.SpriteRenderers;
                int rendererCount = spriteRenderers.Length;
                for (int j = 0; j < rendererCount; j++) {
                    var sr = spriteRenderers[j];
                    if (sr.sortingLayerName != targetLayer) {
                        sr.sortingLayerName = targetLayer;
                    }
                }
            }
        }

        public static void UnRegisterLayerChanger(LayerChangerData layerChanger) {
            if (_instance == null) return;
            
            _instance._layerChangers.Remove(layerChanger);
        }
    }
    
    public class LayerChangerData {
        public LayerChanger Changer;
        public float YOffset;
        
        public LayerChangerData(LayerChanger changer, float yOffset) {
            Changer = changer;
            YOffset = yOffset;
        }
    }
}