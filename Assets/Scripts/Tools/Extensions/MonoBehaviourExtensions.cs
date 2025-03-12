using AppCore;
using AppCore.ParticlesManagement;
using UnityEngine;

namespace Tools {
    public static class MonoBehaviourExtensions {
        public static void CreateParticles(this MonoBehaviour monoBehaviour, GameObject particles) {
            App.Get<ParticlesManager>().SpawnParticles(particles, monoBehaviour.transform);
        }
        
        public static void CreateParticles(this MonoBehaviour monoBehaviour, GameObject particles, Vector3 position) {
            App.Get<ParticlesManager>().SpawnParticles(particles, position);
        }
        
        public static void CreateParticles(this MonoBehaviour monoBehaviour, GameObject particles, Vector3 position, float rotation) {
            App.Get<ParticlesManager>().SpawnParticles(particles, position, rotation);
        }
        
        public static void CreateParticles(this MonoBehaviour monoBehaviour, GameObject particles, Vector3 position, Quaternion rotation, Vector2 scale) {
            App.Get<ParticlesManager>().SpawnParticles(particles, position, rotation, scale);
        }

        public static void CreateParticles(this MonoBehaviour monoBehaviour, GameObject particles, Vector3 position,
            Vector2 direction) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            App.Get<ParticlesManager>().SpawnParticles(particles, position, angle);
        }
    }
}