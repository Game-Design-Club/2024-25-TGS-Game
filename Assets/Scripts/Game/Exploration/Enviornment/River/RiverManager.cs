using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Exploration.Enviornment.River {
    public class RiverManager : MonoBehaviour {
        [SerializeField] private Transform logParent;
        [SerializeField] private Transform rockParent;
        [SerializeField] private Transform spriteParent;
        [Range(1, 20)]
        [SerializeField] private int length = 5;
        [Range(.1f, 10f)]
        [SerializeField] private float size = 2f;
        [Range(0, 5f)]
        [SerializeField] private float sizeBuffer = 0.3f;
        [SerializeField] private float overlap = 0.1f;
        [SerializeField] private BoxCollider2D[] riverColliders;
        [Header("Visuals")]
        [SerializeField] private GameObject baseSpriteObject;
        [SerializeField] private Transform spriteVisualization;
        [SerializeField] private RiverChunk moveSpeedGetter;
        
        private List<BoxCollider2D> _addedColliders = new();
        private GameObject[] _sprites;

        private float _offset;
        
        private void Start() {
            SpriteRenderer baseSpriteRenderer = baseSpriteObject.GetComponent<SpriteRenderer>();
            _offset = baseSpriteRenderer.bounds.size.x - overlap;

            Destroy(spriteVisualization.gameObject);
            SetColliderSizes();
            ComputeColliderRemovals();
            CreateSprites();
        }

        private void OnValidate() {
            transform.localScale = new Vector3(size, size, 1);
            SpriteRenderer spriteRenderer = baseSpriteObject.GetComponent<SpriteRenderer>();
            spriteVisualization.localScale = new Vector3(
                spriteRenderer.bounds.size.x * length * transform.lossyScale.x,
                spriteRenderer.bounds.size.y * transform.lossyScale.y,
                1);
            SetColliderSizes();
        }

        private void SetColliderSizes() {
            SpriteRenderer baseSpriteRenderer = baseSpriteObject.GetComponent<SpriteRenderer>();
            foreach (BoxCollider2D collider in riverColliders) {
                if (collider == null) continue;
                collider.size = new Vector2(
                    baseSpriteRenderer.bounds.size.x * length,
                    baseSpriteRenderer.bounds.size.y - sizeBuffer);
            }
        }

        private void RemoveColliderArea(Transform child) {
            BoxCollider2D addedCollider = gameObject.AddComponent<BoxCollider2D>();
            BoxCollider2D originCollider = child.GetComponent<BoxCollider2D>();
            Vector2 originSize = originCollider.size;
            Vector3 lossyScale = transform.lossyScale;
            
            Vector2 effectiveSize = new Vector2(originSize.x * child.localScale.x / lossyScale.x, originSize.y * child.localScale.y / lossyScale.y);
            addedCollider.size = effectiveSize;

            Vector2 position = child.position;
            if (child.TryGetComponent(out Rigidbody2D rb)) {
                position = rb.position;
            }
            
            addedCollider.offset = transform.InverseTransformPoint(position);
            addedCollider.compositeOperation = Collider2D.CompositeOperation.Difference;
            _addedColliders.Add(addedCollider);
        }

        public void ComputeColliderRemovals() {
            foreach (BoxCollider2D collider in _addedColliders) {
                Destroy(collider);
            }
            _addedColliders.Clear();

            foreach (Transform child in logParent) {
                RemoveColliderArea(child);
            }

            foreach (Transform child in rockParent) {
                RemoveColliderArea(child);
            }
        }
        
        private void CreateSprites() {
            _sprites = new GameObject[length];
            float currentOffset = -_offset*length / 2;
            for (int i = 0; i < length; i++) {
                GameObject instance = Instantiate(baseSpriteObject, spriteParent);
                _sprites[i] = instance;
                _sprites[i].transform.localPosition = new Vector3(currentOffset, 0, 0);
                currentOffset += _offset;
            }
        }
        
        private void Update() {
            for (int i = 0; i < length; i++) {
                // Convert movement direction to local space and calculate wrap threshold
                Vector2 localDir = transform.InverseTransformDirection(moveSpeedGetter.Direction);
                float half = _offset * length / 2f;

                Rigidbody2D rb = _sprites[i].GetComponent<Rigidbody2D>();
                rb.position += moveSpeedGetter.Direction * (moveSpeedGetter.floatSpeed * Time.deltaTime);

                // Wrap sprites along local axes when they move beyond the threshold
                Vector3 localPos = _sprites[i].transform.localPosition;

                if (localDir.x > 0 && localPos.x > half)
                    localPos.x = -half;
                else if (localDir.x < 0 && localPos.x < -half)
                    localPos.x = half;

                if (localDir.y > 0 && localPos.y > half)
                    localPos.y = -half;
                else if (localDir.y < 0 && localPos.y < -half)
                    localPos.y = half;

                _sprites[i].transform.localPosition = localPos;
            }
        }
    }
}