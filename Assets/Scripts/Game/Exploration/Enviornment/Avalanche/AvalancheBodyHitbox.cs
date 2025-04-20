using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.Avalanche {
    public class AvalancheBodyHitbox : MonoBehaviour {
        [SerializeField] private AvalancheManager avalancheManager;
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent(out ChildController child)) {
                avalancheManager.PlayerDie(child);  
            }
        }
    }
}