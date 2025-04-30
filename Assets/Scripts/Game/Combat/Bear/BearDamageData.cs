
using UnityEngine;

namespace Game.Combat.Bear {
    public class BearDamageData {
        public BearDamageData(int damage, Vector2 hitDirection, Vector2 hitPosition, float knockbackForce, BearDamageType damageType) {
            Damage = damage;
            HitDirection = hitDirection;
            HitPosition = hitPosition;
            KnockbackForce = knockbackForce;
            DamageType = damageType;
        }
        
        public int Damage;
        public Vector2 HitDirection;
        public Vector2 HitPosition;
        public float KnockbackForce;
        public BearDamageType DamageType;
    }
}