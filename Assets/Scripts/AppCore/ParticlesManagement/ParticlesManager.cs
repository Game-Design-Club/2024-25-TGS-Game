using UnityEngine;

namespace AppCore.ParticlesManagement {
    public class ParticlesManager : AppModule {
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