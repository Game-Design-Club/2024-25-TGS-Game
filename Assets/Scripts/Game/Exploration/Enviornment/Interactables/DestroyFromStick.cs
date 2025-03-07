using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables {
    public class DestroyFromStick : MonoBehaviour, IChildHittable {
        public void Hit() {
            Destroy(gameObject);
        }
    }
}