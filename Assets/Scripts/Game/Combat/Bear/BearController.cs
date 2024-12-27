using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Bear {
    public class BearController : MonoBehaviour {
        [SerializeField] internal float idleWalkSpeed = 5f;
        
        internal Animator Animator;
        internal Rigidbody2D Rigidbody2D;

        private void Awake() {
            TryGetComponent(out Animator);
            TryGetComponent(out Rigidbody2D);
        }
    }
}