using System;
using System.Collections.Generic;
using Game.GameManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Particles {
    public class EnvironmentParticles : MonoBehaviour {
        [SerializeField] private Transform exploreParticles;
        [SerializeField] private Transform combatParticles;

        private List<ParticleSystem> _exploreParticles = new();
        private List<ParticleSystem> _combatParticles = new();

        private void Awake() {
            foreach (Transform child in exploreParticles) {
                if (child.TryGetComponent(out ParticleSystem ps)) {
                    _exploreParticles.Add(ps);
                }
            }
            foreach (Transform child in combatParticles) {
                if (child.TryGetComponent(out ParticleSystem ps)) {
                    _combatParticles.Add(ps);
                }
            }
        }

        private void OnEnable() {
            GameManager.OnGameEvent += HandleGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= HandleGameEvent;
        }

        private void HandleGameEvent(GameEvent obj) {
            if (obj.GameEventType == GameEventType.ExploreEnter) {
                foreach (ParticleSystem ps in _exploreParticles) {
                    ps.Play();
                }
                foreach (ParticleSystem ps in _combatParticles) {
                    ps.Stop();
                }
            } else if (obj.GameEventType == GameEventType.CombatEnter) {
                foreach (ParticleSystem ps in _combatParticles) {
                    ps.Play();
                }

                foreach (ParticleSystem ps in _exploreParticles) {
                    ps.Stop();
                }
            }
        }
    }
}