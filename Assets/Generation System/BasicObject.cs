using System;
using UnityEngine;

namespace Generation_System
{
    [CreateAssetMenu(fileName = "New Basic Object", menuName = "Basic Object")]
    public class BasicObject : ScriptableObject
    {
        [SerializeField] 
        public GameObject gameObject;
        // [SerializeField] public BasicObjectRenderer[] renderers;
    }

    // [Serializable]
    // public class BasicObjectRenderer
    // {
    //     [SerializeField] public SpriteRenderer renderer;
    //     [SerializeField] public BasicObjectSprite[] spriteOptions;
    // }
    //
    // [Serializable]
    // public class BasicObjectSprite
    // {
    //     [SerializeField] public Sprite sprite;
    //     [SerializeField] public float selectionWeight;
    // }
}