using System;
using UnityEngine;

namespace Game.Exploration.Child {
    public class ChildStick : MonoBehaviour {
        [SerializeField] private ChildController childController;
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent(out IChildHittable hittable)) {
                hittable.Hit((transform.position - childController.transform.position).normalized);
            }
        }
    }
}