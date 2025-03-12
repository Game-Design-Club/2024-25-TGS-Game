using UnityEngine;

namespace Game.Exploration.Child {
    public interface IChildHittable {
        public void Hit(Vector2 hitDirection);
    }
}