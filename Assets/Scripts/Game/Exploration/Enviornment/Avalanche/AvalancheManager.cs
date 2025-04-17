using System;
using System.Collections.Generic;
using Game.GameManagement;
using Tools;
using Tools.CameraShaking;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Exploration.Enviornment.Avalanche {
    public class AvalancheManager : MonoBehaviour {
        [SerializeField] private CinemachineCamera initialCamera;
        [SerializeField] private CinemachineCamera lookCamera;
        [SerializeField] private CinemachineCamera moveCamera;
        [SerializeField] private Transform snowCoverParticles;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Transform movePoint;

        private List<ParticleSystem> _snowParticles = new();
        
        private Animator _animator;

        private bool _moving;
        
        private float _coveredDistance;

        private float _totalDistance;

        private void Awake() {
            _totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
            TryGetComponent(out _animator);
            
            if (snowCoverParticles != null) {
                foreach (Transform child in snowCoverParticles) {
                    if (child.TryGetComponent(out ParticleSystem particle)) {
                        _snowParticles.Add(particle);
                    }
                }
            }
            
            movePoint.position = startPoint.position;
        }
        
        public void StartAvalanche() {
            GameManager.StartCutscene();
            _animator.SetTrigger(AnimationConstants.Avalanche.Start);
            // Block player bounds
        }

        public void AnimStartShaking() {
            SetCamera(initialCamera);
        }
        
        public void AnimStartSnowCover() {
            foreach (ParticleSystem particle in _snowParticles) {
                particle.Play();
            }
        }
        
        public void AnimPanUp() {
            SetCamera(lookCamera);
        }
        
        public void StartMoving() {
            GameManager.EndCutscene();
            _moving = true;
            SetCamera(moveCamera);
        }

        private void Update() {
            if (_moving) {
                _coveredDistance += Time.deltaTime * moveSpeed;
                float percentCovered = _coveredDistance / _totalDistance;
                movePoint.position = Vector2.Lerp(startPoint.position, endPoint.position, percentCovered);
                
                if (percentCovered >= 1f) {
                    StopAvalanche();
                }
            }
        }

        private void StopAvalanche() {
            _moving = false;
            SetCamera(null);
            foreach (ParticleSystem particle in _snowParticles) {
                particle.Stop();
            }
        }

        private void SetCamera(CinemachineCamera cam) {
            initialCamera.Priority = 0;
            moveCamera.Priority = 0;
            lookCamera.Priority = 0;
            if (cam != null) cam.Priority = 100;
        }
    }
}