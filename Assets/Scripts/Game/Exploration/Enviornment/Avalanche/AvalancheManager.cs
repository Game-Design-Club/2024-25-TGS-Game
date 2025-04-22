using System;
using System.Collections.Generic;
using Game.Exploration.Cameras;
using Game.Exploration.Child;
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
        [SerializeField] private AvalancheRockSpawner rockSpawner;
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Transform movePoint;
        [SerializeField] private ChildFollowPoint childFollowPoint;

        private List<ParticleSystem> _snowParticles = new();
        
        private Animator _animator;

        private bool _moving;
        
        private float _coveredDistance;
        private float _totalDistance;
        [HideInInspector] public float percentCovered;

        private ChildController _child;
        
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
            
            if (!_animator) TryGetComponent(out _animator);
            _animator.SetTrigger(AnimationParameters.Avalanche.Start);
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
        
        public void AnimRockSpawning() {
            rockSpawner.StartSpawning(this);
        }
        
        public void AnimStartMoving() {
            GameManager.GameEventType = GameEventType.Explore;
            _moving = true;
            SetCamera(moveCamera);
        }
        
        public void PlayerDie(ChildController child) {
            _animator.SetTrigger(AnimationParameters.Avalanche.Reset);
            child.ExploreDie();
            _child = child;
        }
        
        public void UnlockChildCamera() {
            childFollowPoint.lockX = false;
            childFollowPoint.lockY = false;
            rockSpawner.StopSpawning();
        }

        public void ResetChildCamera() {
            SetCamera(null);
        }
        
        public void AnimReset() {
            movePoint.position = startPoint.position;
            _coveredDistance = 0f;
            percentCovered = 0f;
            _child.transform.position = playerSpawnPoint.position;
            _child.UnDie();
            rockSpawner.StopSpawning();
            _moving = false;
        }

        private void Update() {
            if (_moving) {
                _coveredDistance += Time.deltaTime * moveSpeed;
                percentCovered = _coveredDistance / _totalDistance;
                movePoint.position = Vector2.Lerp(startPoint.position, endPoint.position, percentCovered);
                
                if (percentCovered >= 1f) {
                    StopAvalanche();
                }
            }
        }

        private void StopAvalanche() {
            _moving = false;
            foreach (ParticleSystem particle in _snowParticles) {
                particle.Stop();
            }
            rockSpawner.StopSpawning();
        }

        private void SetCamera(CinemachineCamera cam) {
            initialCamera.Priority = 0;
            moveCamera.Priority = 0;
            lookCamera.Priority = 0;
            if (cam != null) cam.Priority = 100;
        }
    }
}