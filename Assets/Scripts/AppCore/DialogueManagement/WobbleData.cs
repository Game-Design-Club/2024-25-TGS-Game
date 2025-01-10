using UnityEngine;
using UnityEngine.Serialization;

namespace AppCore.DialogueManagement {
    [System.Serializable]
    public class WobbleData {
        [SerializeField] public string name;
        [SerializeField] public float xAmplitude;
        [SerializeField] public float xSpeed;
        [FormerlySerializedAs("horOffset")] [SerializeField] public float xOffset;
        [SerializeField] public float xVertMultiplier;
        [SerializeField] public float xNoise;
        [SerializeField] public float yAmplitude;
        [SerializeField] public float ySpeed;
        [FormerlySerializedAs("vertOffset")] [SerializeField] public float yOffset;
        [SerializeField] public float yVertMultiplier;
        [SerializeField] public float yNoise;

    }
}