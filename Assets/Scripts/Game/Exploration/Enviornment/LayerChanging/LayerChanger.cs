using System;
using System.Linq;
using UnityEngine;

namespace Game.Exploration.Enviornment.LayerChanging {
    public class LayerChanger : MonoBehaviour {
        [SerializeField] public float yOffset = 0f;
        [SerializeField] public bool isStaticLayer = true;
        internal SpriteRenderer[] SpriteRenderers;

        private void OnValidate() {
            SetRenderers();
            SetLayer();
        }

        private void SetRenderers() {
            SpriteRenderer selfRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();

            SpriteRenderers = selfRenderer != null ? childRenderers.Concat(new[] { selfRenderer }).ToArray() : childRenderers;
        }

        private void Start() {
            SetRenderers();
            SetLayer();
        }

        private void Update() {
            if (!isStaticLayer) {
                SetLayer();
            }
        }

        private void SetLayer() {
            foreach (var sr in SpriteRenderers) {
                sr.sortingOrder = Mathf.RoundToInt((transform.position.y+yOffset) * 100f) * -1;
            }
        }
    }
}