using UnityEngine;

namespace AppCore.DialogueManagement {
    [CreateAssetMenu(fileName = "New Dialogue Character", menuName = "Dialogue/Dialogue Character")]
    public class DialogueCharacter : ScriptableObject {
        [SerializeField] public string characterName = "Unnamed Potato Head";
        [SerializeField] public Sprite characterSprite;
        [SerializeField] public Color textColor = Color.white;
        [SerializeField] public TextAlignment textAlignment = TextAlignment.Left;
    }
}