using System;
using UnityEngine;

namespace Game.Exploration.Enviornment.River {
    public class RiverChunk : MonoBehaviour {
        [SerializeField] public Vector2 direction;
        [SerializeField] private Transform directionSprite;
        [SerializeField] public float floatSpeed = 3f;

        private void OnValidate() {
            directionSprite.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        }

        private void Awake() {
            directionSprite.gameObject.SetActive(false);
        }
    }
}