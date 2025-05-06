using System;
using System.Collections.Generic;
using AppCore;
using AppCore.InputManagement;
using Game.GameManagement;
using UnityEngine;

namespace Tools.Debugging {
    public class HotkeyTeleporting : MonoBehaviour {
        [SerializeField] private bool isEnabled = false;

        private List<Transform> _targets = new();

        private void Start() {
            foreach (Transform child in transform) {
                _targets.Add(child);
            }
        }
        
        private void Update() {
            if (!isEnabled) return;
            if (Input.GetKeyDown(KeyCode.BackQuote)) {
                TeleportToTarget(LevelManager.GetCurrentLevel().spawnPoint.position);
            } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                TeleportToTarget(0);
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                TeleportToTarget(1);
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                TeleportToTarget(2);
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                TeleportToTarget(3);
            } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                TeleportToTarget(4);
            } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
                TeleportToTarget(5);
            } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
                TeleportToTarget(6);
            } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
                TeleportToTarget(7);
            } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
                TeleportToTarget(8);
            } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
                TeleportToTarget(9);
            }
        }

        private void TeleportToTarget(int n) {
            if (n >= _targets.Count) return;
            LevelManager.GetCurrentLevel().child.transform.position = _targets[n].position;
        }

        private void TeleportToTarget(Vector2 position) {
            LevelManager.GetCurrentLevel().child.transform.position = position;
        }
    }
}