using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.LevelDesign {
    [Serializable]
    public class SpriteChoosingObject {
        public Sprite sprite;
        public Vector2 size = Vector2.one;
        public Vector2 offset = Vector2.zero;
        public float YOffset = 0f;
    }
}