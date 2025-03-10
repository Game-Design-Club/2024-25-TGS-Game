using UnityEngine;

namespace Tools.LevelDesign {
    [CreateAssetMenu(fileName = "TreeChooserData", menuName = "Tools/LevelDesign/TreeChoserData")]
    public class TreeChoserData : ScriptableObject {
        [SerializeField] public Sprite tree1;
        [SerializeField] public Sprite tree2;
        [SerializeField] public Sprite tree3;
        [SerializeField] public Sprite tree4;
    }
}