using UnityEngine;

namespace Game.Combat.Enemies {
    public interface IBearHittable {
        public GameObject GameObject { get; }
        public void OnHitByBear(int damage, Vector2 hitDirection, float knockbackForce, BearDamageType damageType);
    }
}