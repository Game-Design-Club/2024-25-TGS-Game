using System;

using UnityEngine;

namespace AppCore.DialogueManagement {
    [Serializable]
    public class DialogueChunk {
        [SerializeField] public DialogueCharacter character;
        [TextArea(3, 10)]
        [SerializeField] public String text;
    }
}