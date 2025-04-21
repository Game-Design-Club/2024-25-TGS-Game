using System;
using Game.Exploration.Child;
using Game.GameManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Avalanche {
    public class AvalancheBodyHitbox : MonoBehaviour {
        [SerializeField] private AvalancheManager avalancheManager;
        [SerializeField] private bool removeSpriteOnRun = true;

        private void Awake() {
            if (removeSpriteOnRun) {
                Destroy(GetComponent<SpriteRenderer>());
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent(out ChildController child)) {
                avalancheManager.PlayerDie(child);  
                GameManager.StartCutscene();
            }
        }
    }
}