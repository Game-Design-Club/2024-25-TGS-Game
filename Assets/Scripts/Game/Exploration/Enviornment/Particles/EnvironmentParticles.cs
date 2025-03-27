using System;
using Game.GameManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Particles {
    public class EnvironmentParticles : MonoBehaviour {
        [SerializeField] private ParticleSystem windParticles;
        [SerializeField] private ParticleSystem snowParticles;
        [SerializeField] private ParticleSystem blackSnowParticles;

        private void OnEnable() {
            GameManager.OnGameEvent += HandleGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= HandleGameEvent;
        }

        private void HandleGameEvent(GameEvent obj) {
            if (obj.GameEventType == GameEventType.Explore) {
                windParticles?.Play();
                snowParticles?.Play();
                blackSnowParticles?.Stop();
            } else if (obj.GameEventType == GameEventType.Combat) {
                windParticles?.Stop();
                snowParticles?.Stop();
                blackSnowParticles?.Play();
            }
        }
    }
}