using System;
using Game.GameManagement;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Exploration.Cameras {
    public class ChildFollowCam : MonoBehaviour {
        private CinemachineCamera _cinemachineCamera;
        
        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
        }

        private void Awake() {
            TryGetComponent(out _cinemachineCamera);
        }

        private void Start() {
            _cinemachineCamera.Target.TrackingTarget = LevelManager.GetCurrentLevel().child.transform;
            _cinemachineCamera.ForceCameraPosition(
                LevelManager.GetCurrentLevel().child.transform.position,
                _cinemachineCamera.transform.rotation);
        }

        private void OnGameEvent(GameEvent gameEvent) {
            if (gameEvent.GameEventType == GameEventType.ExploreEnter) {
                _cinemachineCamera.Target.TrackingTarget = LevelManager.GetCurrentLevel().child.transform;
            }
        }
    }
}