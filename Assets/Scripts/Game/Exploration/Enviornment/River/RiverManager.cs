using System.Collections.Generic;
using UnityEngine;

namespace Game.Exploration.Enviornment.River
{
    public class RiverManager : MonoBehaviour
    {
        [SerializeField] private Transform logParent;
        [SerializeField] private Transform rockParent;
        [SerializeField] private Transform spriteParent;
        [Range(1, 250)] [SerializeField] private int length = 5;
        [Range(.1f, 10f)] [SerializeField] private float size = 2f;
        [Range(0, 5f)] [SerializeField] private float sizeBuffer = 0.3f;
        [SerializeField] private float offset = 14f;
        [SerializeField] private BoxCollider2D[] riverColliders;

        [Header("Visuals")]
        [SerializeField] private GameObject baseSpriteObject;
        [SerializeField] private Transform spriteVisualization;
        [SerializeField] private RiverChunk moveSpeedGetter;

        private List<BoxCollider2D> _addedColliders = new();
        private GameObject[] _sprites;

        private Vector2 _calculatedMoveVector;
        private Vector2 _start;
        private Vector2 _end;

        private void Start()
        {
            // Calculate the spacing between sprites
            transform.localScale = new Vector3(size, size, 1);
            
            // SpriteRenderer spriteRenderer = baseSpriteObject.GetComponent<SpriteRenderer>();
            // float prevRotation = transform.rotation.eulerAngles.z;
            // _offset = new Vector2(
            //     spriteRenderer.bounds.size.x * transform.lossyScale.x,
            //     spriteRenderer.bounds.size.y * transform.lossyScale.y);
            
            Destroy(spriteVisualization.gameObject);
            SetColliderSizes();
            ComputeColliderRemovals();
            CreateSprites();
            
        }

        private void OnValidate()
        {
            // Keep scale and collider sizes in sync while editing
            transform.localScale = new Vector3(size, size, 1);
            SpriteRenderer spriteRenderer = baseSpriteObject.GetComponent<SpriteRenderer>();
            spriteVisualization.localScale = new Vector3(
                spriteRenderer.bounds.size.x * length * transform.lossyScale.x,
                spriteRenderer.bounds.size.y * transform.lossyScale.y,
                1);
            SetColliderSizes();
        }

        private void SetColliderSizes()
        {
            // Resize each river collider to match the total sprite area
            SpriteRenderer baseSpriteRenderer = baseSpriteObject.GetComponent<SpriteRenderer>();
            foreach (BoxCollider2D collider in riverColliders)
            {
                if (collider == null) continue;
                collider.size = new Vector2(
                    baseSpriteRenderer.bounds.size.x * length,
                    baseSpriteRenderer.bounds.size.y - sizeBuffer);
            }
        }

        private void RemoveColliderArea(Transform child)
        {
            // Subtract the child’s collider area from the composite collider
            BoxCollider2D origin = child.GetComponent<BoxCollider2D>();
            Vector3 lossyScale = transform.lossyScale;
            Vector2 effectiveSize = new Vector2(
                origin.size.x * child.localScale.x / lossyScale.x,
                origin.size.y * child.localScale.y / lossyScale.y);

            if (child.rotation.z is not 0 or 180) {
                effectiveSize = new Vector2(effectiveSize.y, effectiveSize.x);
            }

            BoxCollider2D added = gameObject.AddComponent<BoxCollider2D>();
            added.size = effectiveSize;

            Vector2 worldPos = child.TryGetComponent<Rigidbody2D>(out var rb) ? rb.position : (Vector2)child.position;
            added.offset = transform.InverseTransformPoint(worldPos);
            added.compositeOperation = Collider2D.CompositeOperation.Difference;
            _addedColliders.Add(added);

        }

        public void ComputeColliderRemovals()
        {
            // Clear and rebuild subtracted collider regions
            foreach (var col in _addedColliders) Destroy(col);
            _addedColliders.Clear();

            foreach (Transform child in logParent) RemoveColliderArea(child);
            foreach (Transform child in rockParent) RemoveColliderArea(child);
            GetComponent<CompositeCollider2D>().GenerateGeometry();
            CompositeCollider2D cc = GetComponent<CompositeCollider2D>();
            cc.GenerateGeometry();
            cc.enabled = false;
            cc.enabled = true;
        }

        private void CreateSprites()
        {
            _sprites = new GameObject[length];

            float rot = transform.rotation.eulerAngles.z;
            _calculatedMoveVector = new Vector2(Mathf.Cos(rot * Mathf.Deg2Rad),Mathf.Sin(rot * Mathf.Deg2Rad)) * offset * size;
            
            _start = (Vector2)transform.position - _calculatedMoveVector * length / 2f;
            
            Vector2 position = _start;

            for (int i = 0; i < length; i++)
            {
                GameObject instance = Instantiate(baseSpriteObject, spriteParent);
                _sprites[i] = instance;
                instance.transform.position = position;
                position += _calculatedMoveVector;
            }

            _end = position;
        }

        private void Update()
        {
            float moveDistance = -moveSpeedGetter.floatSpeed * Time.deltaTime;
            Vector2 moveStep = _calculatedMoveVector.normalized * moveDistance;

            for (int i = 0; i < _sprites.Length; i++)
            {
                GameObject sprite = _sprites[i];
                sprite.transform.position += (Vector3)moveStep;

                Vector2 toStart = (Vector2)sprite.transform.position - _start;
                float projection = Vector2.Dot(toStart, _calculatedMoveVector.normalized);

                float totalDistance = _calculatedMoveVector.magnitude * length;

                if (projection < 0)
                {
                    sprite.transform.position += (Vector3)(_calculatedMoveVector * length);
                }
                else if (projection > totalDistance)
                {
                    sprite.transform.position -= (Vector3)(_calculatedMoveVector * length);
                }
            }
        }
    }
}