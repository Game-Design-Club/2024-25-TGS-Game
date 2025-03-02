using AppCore;
using AppCore.InputManagement;
using UnityEngine;

namespace Game.Exploration.Child {
    public class ChildInteractions : MonoBehaviour {
        [SerializeField] private float interactionLeaveTime = 0.3f;
        
        private Interactable _interactable;
        private bool _interacting = false;
        private bool _hovering = false;
        
        private void OnEnable() {
            App.Get<InputManager>().OnChildInteract += Interact;
        }
        
        private void OnDisable() {
            App.Get<InputManager>().OnChildInteract -= Interact;
        }
        
        private void Interact() {
            if (_interacting) return;
            if (_interactable == null) return;
            _interactable.Interact(InteractionOver);
            _interacting = false;
        }
        
        private void InteractionOver() {
            _interacting = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (_hovering) return;
            _interactable = other.GetComponent<Interactable>();
            if (_interactable == null) return;
            _interactable.Hover();
            _hovering = true;
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            Interactable trigger = other.GetComponent<Interactable>();
            if (trigger == null) return;
            _interactable?.Unhover();
            _interactable = null;
            _hovering = false;
        }
    }
}