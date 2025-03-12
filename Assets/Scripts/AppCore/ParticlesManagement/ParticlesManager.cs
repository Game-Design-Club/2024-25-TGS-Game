using UnityEngine;

namespace AppCore.ParticlesManagement {
    public class ParticlesManager : AppModule {
        public void SpawnParticles(GameObject particles, Vector3 position) {
            SpawnParticles(particles, position, 0);
        }
        
        public void SpawnParticles(GameObject particles, Vector3 position, float rotation) {
            if (particles == null) return;
            Instantiate(particles, position, Quaternion.Euler(0, 0, rotation), transform);
        }
        
        public void SpawnParticles(GameObject particles, Vector3 position, Quaternion rotation, Vector2 scale) {
            if (particles == null) return;
            Instantiate(particles, position, rotation, transform).transform.localScale = scale;
        }

        public void SpawnParticles(GameObject particles, Transform t) {
            SpawnParticles(particles, t.position);
        }
    }
}