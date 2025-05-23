using Game.Exploration.Child;
using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Exploration.Enviornment.Interactables {
    public class DestroyFromStick : MonoBehaviour, IChildHittable {
        [SerializeField] private Transform[] baseParts;
        [SerializeField] private GameObject destroyedBaseParticles;
        [SerializeField] private GameObject destroyedHitParticles;
        [SerializeField] private UnityEvent onHit;
        public void Hit(Vector2 hitDirection) {
            Destroy(gameObject);
            foreach (Transform basePart in baseParts) {
                this.CreateParticles(destroyedBaseParticles, basePart.position, hitDirection);
            }
            this.CreateParticles(destroyedHitParticles, transform.position, hitDirection);
            onHit.Invoke();
        }
    }
}