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
            if (!_controller.CanInteract()) return;
            
            // Origin starts from edge of box collider
            // vertical left first, then vertical right, then horizontal top, then horizontal bottom
            Vector2 direction = new Vector2(_controller.LastDirection.x, 0);
            Vector2 origin = transform.position;
            origin.x += _controller.LastDirection.x * boxCollider.size.x / 2;
            origin.y += boxCollider.size.y / 2;
            TryInteraction(origin, direction); // vertical left
            origin.y -= boxCollider.size.y;
            TryInteraction(origin, direction); // vertical right
            
            direction = new Vector2(0, _controller.LastDirection.y);
            origin = transform.position;
            origin.y += _controller.LastDirection.y * boxCollider.size.y / 2;
            origin.x += boxCollider.size.x / 2;
            TryInteraction(origin, direction); // horizontal top
            origin.x -= boxCollider.size.x;
            TryInteraction(origin, direction); // horizontal bottom
        }
        
        private void TryInteraction(Vector2 origin, Vector2 direction) {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, interactDistance, interactableLayer);
            if (hit.collider == null) return;
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable == null) return;
            interactable.Interact(InteractionOver);
            _interacting = true;
        }
        
        private void InteractionOver() {
            _interacting = false;
        }
    }
}