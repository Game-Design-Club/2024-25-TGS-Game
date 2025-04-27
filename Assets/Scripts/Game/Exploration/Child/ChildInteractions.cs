using System;
using AppCore;
using AppCore.InputManagement;
using UnityEngine;

namespace Game.Exploration.Child {
    public class ChildInteractions : MonoBehaviour {
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float interactDistance = .2f;
        [SerializeField] private BoxCollider2D boxCollider;
        private bool _interacting = false;

        private ChildController _controller;

        private void Awake() {
            TryGetComponent(out _controller);
        }

        private void OnEnable() {
            App.Get<InputManager>().OnChildInteract += Interact;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnChildInteract -= Interact;
        }

        private bool TryRayInteraction(Vector2 baseOffset, Vector2 deltaOffset, Vector2 direction) {
            Vector2 origin = (Vector2)transform.position + baseOffset;
            if (TryInteraction(origin, direction)) return true;
            origin += deltaOffset;
            return TryInteraction(origin, direction);
        }
        
        private void Interact() {
            if (_interacting) return;
            if (!_controller.CanInteract()) return;

            // First try using LastDirection if available
            if (_controller.LastDirection != Vector2.zero) {
                Vector2 lastDir = _controller.LastDirection.normalized;
                if (Mathf.Abs(lastDir.x) > 0.1f) {
                    if (TryRayInteraction(new Vector2(lastDir.x * boxCollider.size.x / 2, boxCollider.size.y / 2), new Vector2(0, -boxCollider.size.y), new Vector2(lastDir.x, 0))) return;
                }
                if (Mathf.Abs(lastDir.y) > 0.1f) {
                    if (TryRayInteraction(new Vector2(boxCollider.size.x / 2, lastDir.y * boxCollider.size.y / 2), new Vector2(-boxCollider.size.x, 0), new Vector2(0, lastDir.y))) return;
                }
            }

            if (_controller.LastInput == Vector2.zero) {
                if (TryRayInteraction(new Vector2(-boxCollider.size.x / 2, boxCollider.size.y / 2), new Vector2(0, -boxCollider.size.y), Vector2.left)) return;
                if (TryRayInteraction(new Vector2(boxCollider.size.x / 2, boxCollider.size.y / 2), new Vector2(0, -boxCollider.size.y), Vector2.right)) return;
                if (TryRayInteraction(new Vector2(boxCollider.size.x / 2, boxCollider.size.y / 2), new Vector2(-boxCollider.size.x, 0), Vector2.up)) return;
                if (TryRayInteraction(new Vector2(boxCollider.size.x / 2, -boxCollider.size.y / 2), new Vector2(-boxCollider.size.x, 0), Vector2.down)) return;
            } else {
                Vector2 moveDir = _controller.LastInput.normalized;
                if (Mathf.Abs(moveDir.x) > 0.1f) {
                    if (TryRayInteraction(new Vector2(moveDir.x * boxCollider.size.x / 2, boxCollider.size.y / 2), new Vector2(0, -boxCollider.size.y), new Vector2(moveDir.x, 0))) return;
                }
                if (Mathf.Abs(moveDir.y) > 0.1f) {
                    if (TryRayInteraction(new Vector2(boxCollider.size.x / 2, moveDir.y * boxCollider.size.y / 2), new Vector2(-boxCollider.size.x, 0), new Vector2(0, moveDir.y))) return;
                }
            }
        }
        
        private bool TryInteraction(Vector2 origin, Vector2 direction) {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, interactDistance, interactableLayer);
            if (hit.collider == null) return false;
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable == null) return false;
            interactable.Interact(InteractionOver);
            _interacting = true;
            return true;
        }
        
        private void InteractionOver() {
            _interacting = false;
        }
    }
}