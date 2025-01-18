using System;
using System.Collections.Generic;
using Game.Exploration.Child;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChangingManager : MonoBehaviour {
        [SerializeField] private ChildController child;
        private List<LayerChanger> _layerChangers = new();

        private static LayerChangingManager _instance;
        
        private void Awake() {
            if (_instance == null) _instance = this;
            else Destroy(gameObject);
            
        }

        public static void RegisterLayerChanger(LayerChanger changer) {
            _instance._layerChangers.Add(changer);
        }

        private void Update() {
            foreach (LayerChanger changer in _layerChangers) {
                if (child.transform.position.y - changer.transform.position.y < 0) {
                    changer.spriteRenderer.sortingLayerName = Constants.Layers.ChildGameplay;
                }
                else {
                    changer.spriteRenderer.sortingLayerName = Constants.Layers.ChildGameplayFront;
                }
            }
        }
    }
}