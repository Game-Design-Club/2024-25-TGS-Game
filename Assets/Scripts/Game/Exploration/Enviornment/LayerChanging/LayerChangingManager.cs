using System;
using System.Collections.Generic;
using Game.Exploration.Child;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChangingManager : MonoBehaviour {
        [SerializeField] private ChildController child;
        private static List<LayerChanger> _layerChangers = new();
        
        public static void RegisterLayerChanger(LayerChanger changer) {
            _layerChangers.Add(changer);
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