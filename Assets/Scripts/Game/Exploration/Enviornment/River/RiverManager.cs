using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Exploration.Enviornment.River {
    public class RiverManager : MonoBehaviour {
        [SerializeField] private Transform logParent;
        [SerializeField] private Transform rockParent;
        
        private List<BoxCollider2D> addedColliders = new List<BoxCollider2D>();
        
        private void Start() {
            ComputeCollider();
        }

        private void AddCollider(Transform child) {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            BoxCollider2D originCollider = child.GetComponent<BoxCollider2D>();
            Vector2 originSize = originCollider.size;
            Vector3 lossyScale = transform.lossyScale;
            
            Vector2 effectiveSize = new Vector2(originSize.x * child.localScale.x / lossyScale.x, originSize.y * child.localScale.y / lossyScale.y);
            collider.size = effectiveSize;

            Vector2 position = child.position;
            if (child.TryGetComponent(out Rigidbody2D rb)) {
                position = rb.position;
            }
            
            collider.offset = transform.InverseTransformPoint(position);
            collider.compositeOperation = Collider2D.CompositeOperation.Difference;
            addedColliders.Add(collider);
        }

        public void ComputeCollider() {
            foreach (BoxCollider2D collider in addedColliders) {
                Destroy(collider);
            }
            addedColliders.Clear();

            foreach (Transform child in logParent) {
                AddCollider(child);
            }

            foreach (Transform child in rockParent) {
                AddCollider(child);
            }
        }
    }
}