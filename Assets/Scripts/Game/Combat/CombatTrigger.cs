using UnityEngine;

namespace Game.Combat {
    public class CombatTrigger : MonoBehaviour {
        [SerializeField] private CombatAreaManager combatAreaManager;

        private bool _hasEntered = false;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (_hasEntered) return;
            if (!other.CompareTag("Child")) return;
            
            _hasEntered = true;
            
            combatAreaManager.EnterCombatArea();
        }
        
    }
}