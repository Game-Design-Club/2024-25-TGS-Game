using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.LevelDesign {
    public class LevelCreator : MonoBehaviour {
        public enum Shape
        {
            Square,
            Circle,
            Ring
        }
        

            [Header("General Settings")]
        [SerializeField] public bool isPlacing = false;
        private bool wasPlacing = false;
        [SerializeField] public bool isErasing = false;
        private bool wasErasing = false;
        [SerializeField] public bool snapToGrid = true;
        [SerializeField] public float gridSize = .5f;
        [SerializeField] public bool fillArea = false;
        [SerializeField] public float areaSize = .5f;
        [FormerlySerializedAs("areaThickness")] [SerializeField] public float thickness = 10f;
        [SerializeField] public Shape areaShape = Shape.Square;
        [SerializeField] public float density = .05f;
        [SerializeField] public LevelCreatorModule[] modules;
        [HideInInspector] public int activeModuleIndex = 0;
        public GameObject ObjectPlacingPrefab => modules[activeModuleIndex].objectPlacingPrefab;
        public Transform ParentTransform => modules[activeModuleIndex].parentTransform;
        public bool TryRandomize => modules[activeModuleIndex].tryRandomize;
        public bool RandomizeFlip => modules[activeModuleIndex].randomizeFlip;
        public bool UseFlags => modules[activeModuleIndex].useFlags;
        public bool[] CustomRandomizeFlags => modules[activeModuleIndex].customRandomizeFlags;

        private void OnValidate()
        {
            if (isErasing && !wasErasing) isPlacing = false;
            if (isPlacing && !wasPlacing) isErasing = false;
            
            if (activeModuleIndex < 0) activeModuleIndex = 0;
            if (activeModuleIndex >= modules.Length) activeModuleIndex = modules.Length - 1;
            for (int i = 0; i < modules.Length; i++) {
                if (i == activeModuleIndex) {
                    modules[i].activeModule = true;
                } else {
                    modules[i].activeModule = false;
                }
            }
            wasPlacing = isPlacing;
            wasErasing = isErasing;
        }
    }
}