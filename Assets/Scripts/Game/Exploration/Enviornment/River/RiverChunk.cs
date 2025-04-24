using System;
using UnityEngine;

namespace Game.Exploration.Enviornment.River {
    public class RiverChunk : MonoBehaviour {
        [SerializeField] private Vector2 direction;
        [SerializeField] private Transform directionSprite;
        [SerializeField] public float floatSpeed = 3f;
        [SerializeField] private BoxCollider2D matchCollider;

        private BoxCollider2D _boxCollider;
        
        public Vector2 Direction {
            get {
                // Use rotation to get the world direction
                Vector2 dirLocal = direction;
                float angleRad = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
                float cos = Mathf.Cos(angleRad);
                float sin = Mathf.Sin(angleRad);
                Vector2 dir = new Vector2(
                    dirLocal.x * cos - dirLocal.y * sin,
                    dirLocal.x * sin + dirLocal.y * cos
                );
                return dir;
            }
        }
        
        private void OnValidate() {
            directionSprite.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, Direction));
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