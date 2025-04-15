using System;
using UnityEngine;

namespace Game.Exploration.Enviornment.River {
    public class RiverChunk : MonoBehaviour {
        [SerializeField] public Vector2 direction;
        [SerializeField] private Transform directionSprite;
        [SerializeField] public float floatSpeed = 3f;
        [SerializeField] private BoxCollider2D matchCollider;

        private BoxCollider2D _boxCollider;
        
        private void OnValidate() {
            directionSprite.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
            TryGetComponent(out _boxCollider);
            if (matchCollider) {
                _boxCollider.size = matchCollider.size;
                _boxCollider.offset = matchCollider.offset;
            }
        }

        private void Awake() {
            directionSprite.gameObject.SetActive(false);
        }
    }
}