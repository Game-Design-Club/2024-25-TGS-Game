using System;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Combat {
    public class CombatTrigger : MonoBehaviour {
        [SerializeField] private CombatAreaManager combatAreaManager;

        private void Start() {
            combatAreaManager.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out ChildController child)) return;
            combatAreaManager.gameObject.SetActive(true);
            combatAreaManager.EnterCombatArea(child);
        }
    }
}