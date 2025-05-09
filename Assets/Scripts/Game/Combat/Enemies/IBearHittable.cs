using Game.Combat.Bear;
using UnityEngine;

namespace Game.Combat.Enemies {
    public interface IBearHittable {
        public GameObject GameObject { get; }
        public void OnHitByBear(BearDamageData data);
    }
}