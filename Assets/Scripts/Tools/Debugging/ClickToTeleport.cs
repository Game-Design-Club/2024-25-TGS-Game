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
                BearController b = FindObjectsByType<BearController>(FindObjectsSortMode.None)[0];
                if (b) {
                    b.transform.position = clickPosition;
                }
            } else {
                ChildController[] c = FindObjectsByType<ChildController>(FindObjectsSortMode.None);
                if (c.Length > 0 && c[0]) {
                    c[0].transform.position = clickPosition;
                }
            }
        }
    }
}