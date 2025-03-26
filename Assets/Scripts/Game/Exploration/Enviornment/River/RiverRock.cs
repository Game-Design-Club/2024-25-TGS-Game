using System;
using System.Collections;
using System.Collections.Generic;
using Game.Exploration.Child;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.River {
    public class RiverRock : MonoBehaviour {
        [SerializeField] private float slideIntoRiverVelocity = 3f;
        [SerializeField] private float landScale = 1.8f;
        [SerializeField] private float targetScale = 1.5f;
        [SerializeField] private float scaleTime = 0.5f;
        [SerializeField] private Transform spriteRenderer;
        
        public bool InRiver { get; private set; } = false;
        private bool _isMoving = false;

        private Rigidbody2D _rb;
        private BoxCollider2D _collider;

        private Vector2 _moveDirection;
        
        private ChildController _childController;
        private Vector2 _lastPushDirection;
        
        private void Awake() {
            TryGetComponent(out _rb);
            TryGetComponent(out _collider);
            CheckRiver();
            if (_isMoving) {
                spriteRenderer.localScale = new Vector3(targetScale, targetScale, 1f);
            } else {
                spriteRenderer.localScale = new Vector3(landScale, landScale, 1f);
            }
        }

        private void CheckRiver() {
            if (InRiver || _isMoving) return;
            
            PointCollision pointCollision = new PointCollision(transform.position, _collider);
            if (pointCollision.TouchingRiverBase && !pointCollision.TouchingLand) {
                _collider.isTrigger = true;
                _rb.bodyType = RigidbodyType2D.Dynamic;
                _rb.linearVelocity = Vector2.zero;
                StartCoroutine(MoveRockToRiver());
                StartCoroutine(MakeSmaller());
            }
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
            new PointCollision(_rb.position).RiverManager?.ComputeCollider();
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
                new PointCollision(_rb.position).RiverManager?.ComputeCollider();
            } else {
                _rb.linearVelocity = Vector2.zero;
            }

            if (!InRiver) {
                CheckRiver();
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            Vector2 collisionNormal = other.contacts[0].normal;
            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y)) {
                _lastPushDirection = new Vector2(1, 0);
            } else {
                _lastPushDirection = new Vector2(0, 1);
            }
        }
    }
}