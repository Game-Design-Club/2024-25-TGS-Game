using System;
using System.Linq;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChanger : MonoBehaviour {
        [SerializeField] public float yOffset = 0f;
        internal SpriteRenderer[] SpriteRenderers;

        private void OnValidate() {
            SetLayer();
        }

        private void SetLayer() {
            SpriteRenderer selfRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();

            SpriteRenderers = selfRenderer != null ? childRenderers.Concat(new[] { selfRenderer }).ToArray() : childRenderers;
            
            foreach (var sr in SpriteRenderers) {
                sr.sortingOrder = Mathf.RoundToInt((transform.position.y+yOffset) * 100f) * -1;
            }
        }
    }
}