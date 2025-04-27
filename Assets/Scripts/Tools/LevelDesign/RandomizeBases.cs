using UnityEngine;

namespace Tools.LevelDesign {
    public class RandomizeBases : MonoBehaviour {
        [SerializeField] private bool randomize;
        [SerializeField] private SpriteChooser[] spriteChoosers;
        
        private void OnValidate() {
            if (randomize) {
                randomize = false;
                foreach (SpriteChooser spriteChooser in spriteChoosers) {
                    spriteChooser.RandomizeSprite();
                }
            }
        }
    }
}
