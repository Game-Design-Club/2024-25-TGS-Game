using UnityEngine;

namespace Tools.LevelDesign {
    [CreateAssetMenu(fileName = "Chooser Data", menuName = "Sprite Chooser Data")]
    public class SpriteChooserData : ScriptableObject {
        [SerializeField] public Sprite[] objects;
    }
}