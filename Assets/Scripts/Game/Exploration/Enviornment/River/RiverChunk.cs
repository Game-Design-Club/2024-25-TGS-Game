using System;
using UnityEngine;

namespace Game.Exploration.Enviornment.River {
    public class RiverChunk : MonoBehaviour {
        [SerializeField] private RiverDirection direction;
        [SerializeField] private Transform directionSprite;

        private void OnValidate() {
            directionSprite.rotation = Quaternion.Euler(0, 0, -(int) direction * 45);
        }

        private void Awake() {
            directionSprite.gameObject.SetActive(false);
        }

        public Vector2 GetDirection => direction switch {
            RiverDirection.Up => Vector2.up,
            RiverDirection.UpRight => new Vector2(1, 1),
            RiverDirection.Right => Vector2.right,
            RiverDirection.DownRight => new Vector2(1, -1),
            RiverDirection.Down => Vector2.down,
            RiverDirection.DownLeft => new Vector2(-1, -1),
            RiverDirection.Left => Vector2.left,
            RiverDirection.UpLeft => new Vector2(-1, 1),
            _ => Vector2.zero
        };
    }
    
    public enum RiverDirection {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }
}