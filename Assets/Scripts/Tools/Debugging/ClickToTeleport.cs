using Game.Combat.Bear;
using Game.Exploration.Child;
using Game.GameManagement;
using UnityEngine;

namespace Tools.Debugging {
    public class ClickToTeleport : MonoBehaviour {
        [SerializeField] private bool isEnabled = false;
        private void Update() {
            if (isEnabled && Input.GetMouseButtonDown(0) && !GameManager.IsPaused) {
                TeleportToTarget();
            }
        }

        private void TeleportToTarget() {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GameManager.GameEventType == GameEventType.Combat) {
                FindObjectsByType<BearController>(FindObjectsSortMode.None)[0].transform.position = clickPosition;
            } else {
                FindObjectsByType<ChildController>(FindObjectsSortMode.None)[0].transform.position = clickPosition;
            }
        }
    }
}