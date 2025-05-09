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
        [SerializeField] public bool isActive = false;
        [SerializeField] public bool isErasing = false;
        [SerializeField] public bool useCurve = false;
        [SerializeField] public AnimationCurve strengthCurve = new AnimationCurve();
        [Range(0f, 1f)][SerializeField] public float strength = 1f;
        [SerializeField] public bool snapToGrid = true;
        [SerializeField] public float gridSize = .5f;
        [SerializeField] public Vector2 gridOffset = new Vector2();
        [SerializeField] public bool useArea = false;
        [SerializeField] public Shape areaShape = Shape.Square;
        [SerializeField] public float areaSize = .5f;
        [SerializeField] public float thickness = 10f;
        [SerializeField] public float density = .05f;
        [HideInInspector] public int activeModuleIndex = 0;
        [SerializeField] public LevelCreatorModule[] modules = Array.Empty<LevelCreatorModule>();
        public GameObject ObjectPlacingPrefab => modules[activeModuleIndex].objectPlacingPrefab;
        public Transform ParentTransform => modules[activeModuleIndex].parentTransform;
        public bool TryRandomize => modules[activeModuleIndex].tryRandomize;
        public bool RandomizeFlip => modules[activeModuleIndex].randomizeFlip;
        public bool UseFlags => modules[activeModuleIndex].useFlags;
        public bool[] CustomRandomizeFlags => modules[activeModuleIndex].customRandomizeFlags;

        private void OnValidate()
        {
            if (isErasing) useArea = true;

            if (activeModuleIndex < 0) activeModuleIndex = 0;
            if (activeModuleIndex >= modules.Length) activeModuleIndex = modules.Length - 1;
            for (int i = 0; i < modules.Length; i++) {
                if (i == activeModuleIndex) {
                    modules[i].activeModule = true;
                } else {
                    modules[i].activeModule = false;
                }
            }

            areaSize = Mathf.Clamp(Mathf.Max(0, areaSize), 0, 100);
            thickness = Mathf.Clamp(thickness, 1, areaSize / 2);
            gridSize = Mathf.Max(0, gridSize);
            gridOffset.x %= gridSize;
            gridOffset.y %= gridSize;
        }
    }
}