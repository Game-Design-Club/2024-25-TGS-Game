using UnityEngine;

namespace AppCore.DialogueManagement {
    [CreateAssetMenu(fileName = "TextEffectsData", menuName = "Dialogue/TextEffectsData")]
    public class TextEffectsData : ScriptableObject {
        [SerializeField] public WobbleData[] wobbleData;
        [SerializeField] public float appearDuration = 0.1f;
        [SerializeField] public float appearHeight = 2f;
    }
}