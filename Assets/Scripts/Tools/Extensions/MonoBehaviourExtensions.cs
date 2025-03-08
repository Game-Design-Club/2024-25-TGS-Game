using AppCore;
using AppCore.ParticlesManagement;
using UnityEngine;

namespace Tools {
    public static class MonoBehaviourExtensions {
        public static void CreateParticles(this MonoBehaviour monoBehaviour, ParticleSystem particles) {
            App.Get<ParticlesManager>().SpawnParticles(particles, monoBehaviour.transform);
        }
        
        public static void CreateParticles(this MonoBehaviour monoBehaviour, ParticleSystem particles, Vector3 position, float rotation) {
            App.Get<ParticlesManager>().SpawnParticles(particles, position, rotation);
        }

        public static void CreateParticles(this MonoBehaviour monoBehaviour, ParticleSystem particles, Vector3 position,
            Vector2 direction) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            App.Get<ParticlesManager>().SpawnParticles(particles, position, angle);
        }
    }
}