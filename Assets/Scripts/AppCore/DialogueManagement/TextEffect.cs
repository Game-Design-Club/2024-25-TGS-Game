using Tools;

namespace AppCore.DialogueManagement {
    public class TextEffect {
        public TextEffectType Type;
        public object Data;
        

        public TextEffect(TextEffectType type, object data) {
            Type = type;
            Data = data;
        }
    }
}