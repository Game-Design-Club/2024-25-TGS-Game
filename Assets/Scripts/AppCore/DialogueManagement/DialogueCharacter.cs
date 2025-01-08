using UnityEngine;
using UnityEngine.Serialization;

namespace AppCore.DialogueManagement {
    [CreateAssetMenu(fileName = "New Dialogue Character", menuName = "Dialogue/Dialogue Character")]
    public class DialogueCharacter : ScriptableObject {
        [SerializeField] public string characterName = "Unnamed Potato Head";
        [FormerlySerializedAs("characterSprite")] [SerializeField] public Sprite sprite;
        [SerializeField] public Color textColor = Color.white;
        [SerializeField] public TextAlignment textAlignment = TextAlignment.Left;
    }
}