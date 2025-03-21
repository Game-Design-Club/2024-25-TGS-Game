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
        
        private void Interact() {
            if (_interacting) return;
            
            // Origin starts from edge of box collider
            Vector2 direction = _controller.LastDirection;
            Vector2 origin = transform.position;
            origin.x += direction.x * boxCollider.size.x / 2;
            origin.y += direction.y * boxCollider.size.y / 2;
            
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, interactDistance, interactableLayer);
            if (hit.collider == null) return;
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable == null) return;
            interactable.Interact(InteractionOver);
            _interacting = false;
        }
        
        private void InteractionOver() {
            _interacting = false;
        }
    }
}