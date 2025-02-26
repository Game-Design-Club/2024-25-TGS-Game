using Game.GameManagement;
using UnityEngine;

namespace Tools {
    public static class MonoBehaviourExtensions {
        public static void CreateParticles(this MonoBehaviour monoBehaviour, ParticleSystem particles) {
            ParticlesManager.Instance.SpawnParticles(particles, monoBehaviour.transform);
        }
        
        public static void CreateParticles(this MonoBehaviour monoBehaviour, ParticleSystem particles, Vector3 position, float rotation) {
            ParticlesManager.Instance.SpawnParticles(particles, position, rotation);
        }

        public static void CreateParticles(this MonoBehaviour monoBehaviour, ParticleSystem particles, Vector3 position,
            Vector2 direction) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            ParticlesManager.Instance.SpawnParticles(particles, position, angle);
        }
    }
}