using UnityEngine;

namespace AppCore.DialogueManagement {
    [CreateAssetMenu(fileName = "TextEffectsData", menuName = "Dialogue/TextEffectsData")]
    public class TextEffectsData : ScriptableObject {
        [SerializeField] public WobbleData[] wobbleData;
    }
}