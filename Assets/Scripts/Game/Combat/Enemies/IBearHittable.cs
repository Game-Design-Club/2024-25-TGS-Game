using UnityEngine;

namespace Game.Combat.Enemies {
    public interface IBearHittable {
        public GameObject GameObject { get; }
        public void OnBearHit(int damage, Vector2 hitDirection, float knockbackForce);
    }
}