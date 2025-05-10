using UnityEngine;
using UnityEngine.Events;

namespace Tools {
    public class ThingTrigger : MonoBehaviour {
        [SerializeField] private bool oneTimeUse = true;
        [SerializeField] private UnityEvent onTriggerEnter;
        [SerializeField] private UnityEvent onTriggerExit;
        
        private bool _triggered;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(Tags.Child)) return;
            if (oneTimeUse && _triggered) return;
            onTriggerEnter?.Invoke();
            _triggered = true;
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (oneTimeUse && _triggered) return;
            onTriggerExit?.Invoke();
            _triggered = false;
        }
    }
}