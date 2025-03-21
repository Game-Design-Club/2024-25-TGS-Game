using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.LevelDesign {
    public class LevelCreator : MonoBehaviour {
        [Header("General Settings")]
        [SerializeField] public bool isPlacing = false;
        [SerializeField] public bool snapToGrid = true;
        [SerializeField] public float gridSize = .5f;
        [SerializeField] public LevelCreatorModule[] modules;
        [HideInInspector] public int activeModuleIndex = 0;
        public GameObject ObjectPlacingPrefab => modules[activeModuleIndex].objectPlacingPrefab;
        public Transform ParentTransform => modules[activeModuleIndex].parentTransform;
        public bool TryRandomize => modules[activeModuleIndex].tryRandomize;
        public bool UseFlags => modules[activeModuleIndex].useFlags;
        public bool[] CustomRandomizeFlags => modules[activeModuleIndex].customRandomizeFlags;

        private void OnValidate() {
            if (activeModuleIndex < 0) activeModuleIndex = 0;
            if (activeModuleIndex >= modules.Length) activeModuleIndex = modules.Length - 1;
            for (int i = 0; i < modules.Length; i++) {
                if (i == activeModuleIndex) {
                    modules[i].activeModule = true;
                } else {
                    modules[i].activeModule = false;
                }
            }
        }
    }
}