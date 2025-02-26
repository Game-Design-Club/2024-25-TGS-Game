using System;
using UnityEngine;

namespace Game.GameManagement {
    public class ParticlesManager : MonoBehaviour {
        public static ParticlesManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                Debug.LogWarning("Multiple ParticlesManagers found in scene. Destroying duplicate.");
            }
        }
        
        public void SpawnParticles(ParticleSystem particles, Vector3 position) {
            SpawnParticles(particles, position, 0);
        }
        
        public void SpawnParticles(ParticleSystem particles, Vector3 position, float rotation) {
            if (particles == null) return;
            Instantiate(particles, position, Quaternion.Euler(0, 0, rotation), transform);
        }

        public void SpawnParticles(ParticleSystem particles, Transform t) {
            SpawnParticles(particles, t.position);
        }
    }
}