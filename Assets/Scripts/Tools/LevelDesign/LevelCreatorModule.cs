using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.LevelDesign {
    [Serializable]
    public class LevelCreatorModule {
        [SerializeField] public bool activeModule;
        [SerializeField] public GameObject objectPlacingPrefab;
        [SerializeField] public Transform parentTransform;
        [SerializeField] public bool tryRandomize = true;
        [SerializeField] public bool useFlags = false;
        [FormerlySerializedAs("randomizeFlags")] [SerializeField] public bool[] customRandomizeFlags;
    }
}