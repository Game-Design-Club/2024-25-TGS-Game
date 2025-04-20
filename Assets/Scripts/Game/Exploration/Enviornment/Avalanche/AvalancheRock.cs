using System;
using Game.Exploration.Child;
using Game.GameManagement;
using Tools;
using UnityEngine;

namespace Game.Exploration.Enviornment.Avalanche {
    public class AvalancheRock : MonoBehaviour {
        private Rigidbody2D _rb;
        
        private void Awake() {
            TryGetComponent(out _rb);
        }
        
        internal void Launch(float force) {
            TryGetComponent(out _rb);
            _rb.linearVelocity = Vector2.down * force;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent(out ChildController child)) {
                HitSomething();
                GameManager.StartCutscene();
            } else if (other.gameObject.layer == PhysicsLayers.ChildWall && 
                       !other.CompareTag(Tags.Avalanche)){
                HitSomething();
            }
        }

        private void HitSomething() {
            Destroy(gameObject);
        }
    }
}