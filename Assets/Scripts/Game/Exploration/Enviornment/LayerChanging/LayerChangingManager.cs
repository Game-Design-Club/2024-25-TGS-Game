using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChangingManager : MonoBehaviour {
        [SerializeField] private Transform child;
        private readonly List<LayerChanger> _layerChangers = new List<LayerChanger>();

        private static LayerChangingManager _instance;
        private float _childY;

        private void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public static void RegisterLayerChanger(LayerChanger changer) {
            if (_instance != null) {
                _instance._layerChangers.Add(changer);
                foreach (var sr in changer.SpriteRenderers) {
                    sr.sortingOrder = Mathf.RoundToInt(changer.transform.position.y * 100f) * -1;
                }
            }
        }

        private void Update() {
            // Cache child's y-coordinate once per frame.
            _childY = child.position.y;
            
            int changerCount = _layerChangers.Count;
            for (int i = 0; i < changerCount; i++) {
                var changer = _layerChangers[i];
                bool childIsBehind = (_childY - changer.transform.position.y) < 0;
                string targetLayer = childIsBehind ? Constants.Layers.ChildGameplay : Constants.Layers.ChildGameplayFront;

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
    }
}