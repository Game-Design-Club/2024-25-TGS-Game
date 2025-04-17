using System;
using System.Collections;
using System.Collections.Generic;
using AppCore;
using AppCore.DataManagement;
using Game.Exploration.Child;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Exploration.Enviornment.River {
    public class RiverRock : MonoBehaviour, DataID {
        [FormerlySerializedAs("blockPlayer")] [SerializeField] private bool blockChild = false;
        [SerializeField] private float slideIntoRiverVelocity = 3f;
        [SerializeField] private float landScale = 1.8f;
        [SerializeField] private float targetScale = 1.5f;
        [SerializeField] private float scaleTime = 0.5f;
        [SerializeField] private Transform spriteRenderer;
        [SerializeField] private string flagName;
        [SerializeField] private ParticleSystem splashParticles;
        
        public bool InRiver { get; private set; } = false;
        private bool _isMoving = false;

        private Rigidbody2D _rb;
        private BoxCollider2D _collider;

        private Vector2 _moveDirection;
        
        private ChildController _childController;
        private Vector2 _lastPushDirection;
        
        RiverRockSaveData _saveData;
        
        [ContextMenu("Set Flag Name")]
        public void GenerateID() {
            flagName = name + Guid.NewGuid();
        }

        private void Reset() {
            GenerateID();
        }
        
        private void Awake() {
            TryGetComponent(out _rb);
            TryGetComponent(out _collider);
            if (App.Get<DataManager>().TryGetCustomData(flagName, out object data)) {
                _saveData = (RiverRockSaveData)data;
                InRiver = _saveData.inRiver;
                _rb.position = _saveData.position;
            } else {
                App.Get<DataManager>().SetCustomData(flagName, new RiverRockSaveData {
                    position = _rb.position,
                    inRiver = InRiver
                });
            }
            if (InRiver) {
                spriteRenderer.localScale = new Vector3(targetScale, targetScale, 1f);
                SetInRiver();
                new PointCollision(_rb.position).RiverManager?.ComputeColliderRemovals();
            } else {
                spriteRenderer.localScale = new Vector3(landScale, landScale, 1f);
            }
        }
        
        private void CheckRiver() {
            if (InRiver || _isMoving) return;
            
            PointCollision pointCollision = new PointCollision(transform.position, _collider);
            if (pointCollision.TouchingRiverBase && !pointCollision.TouchingLand) {
                SetInRiver();
                StartCoroutine(MoveRockToRiver());
                StartCoroutine(MakeSmaller());
            }
        }

        private void SetInRiver() {
            splashParticles.Play();
            if (!blockChild) {
                _collider.isTrigger = true;
                _rb.bodyType = RigidbodyType2D.Dynamic;
            } else {
                gameObject.layer = PhysicsLayers.ChildWall;
                _rb.bodyType = RigidbodyType2D.Kinematic;
            }
            _rb.linearVelocity = Vector2.zero;
        }

        private IEnumerator MoveRockToRiver() {
            _isMoving = true;
            yield return StartCoroutine(
                PointCollisionHelper.MoveToInArea(
                    _collider,
                    _rb, 
                    dir => _moveDirection = dir, 
                    pc => pc.TouchingRiverBase && !pc.TouchingLand,
                    .1f,
                    _lastPushDirection));
            _isMoving = false;
            InRiver = true;
            new PointCollision(_rb.position).RiverManager?.ComputeColliderRemovals();
        }

        private IEnumerator MakeSmaller() {
            Vector3 originalScale = new Vector3(landScale, landScale, 1f);
            float elapsedTime = 0f;

            while (elapsedTime < scaleTime) {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / scaleTime);
                spriteRenderer.localScale = Vector3.Lerp(originalScale, new Vector3(targetScale, targetScale, 1f), t);
                yield return null;
            }
        }

        private void Update() {
            if (_isMoving) {
                _rb.linearVelocity = _moveDirection * slideIntoRiverVelocity;
                new PointCollision(_rb.position).RiverManager?.ComputeColliderRemovals();
            } else {
                _rb.linearVelocity = Vector2.zero;
            }

            if (!InRiver) {
                CheckRiver();
            }
            
            App.Get<DataManager>().SetCustomData(flagName, new RiverRockSaveData {
                position = _rb.position,
                inRiver = InRiver
            });
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag(Tags.River) || other.gameObject.CompareTag(Tags.RiverBase)) {
                Physics2D.IgnoreCollision(_collider, other.collider);
                return;
            }
            Vector2 collisionNormal = other.contacts[0].normal;
            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y)) {
                _lastPushDirection = new Vector2(1, 0);
            } else {
                _lastPushDirection = new Vector2(0, 1);
            }
        }
    }

    [Serializable]
    public struct RiverRockSaveData {
        public Vector2 position;
        public bool inRiver;
    }
}