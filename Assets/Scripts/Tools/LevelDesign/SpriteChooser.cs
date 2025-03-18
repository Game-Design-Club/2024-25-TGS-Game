using Game.Exploration.Enviornment.LayerChanging;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.LevelDesign {
    public class SpriteChooser : MonoBehaviour {
        [SerializeField] private bool randomize = false;
        [SerializeField] private int objectNumber;
        [FormerlySerializedAs("_spriteChoserData")] [SerializeField] private SpriteChooserData spriteChooserData;

        private void OnValidate() {
            if (randomize) {
                objectNumber = Random.Range(0, spriteChooserData.objects.Length);
                randomize = false;
            }
            if (objectNumber >= spriteChooserData.objects.Length) {
                objectNumber = spriteChooserData.objects.Length - 1;
            }
            if (objectNumber < 0) {
                objectNumber = 0;
            }
            if (TryGetComponent(out SpriteRenderer spriteRenderer)) {
                spriteRenderer.sprite = spriteChooserData.objects[objectNumber].sprite;
                if (TryGetComponent(out BoxCollider2D boxCollider2D)) {
                    boxCollider2D.size = spriteChooserData.objects[objectNumber].size;
                    boxCollider2D.offset = spriteChooserData.objects[objectNumber].offset;
                }
                if (TryGetComponent(out LayerChanger layerChanger)) {
                    layerChanger.yOffset = spriteChooserData.objects[objectNumber].YOffset;
                }
            } else {
                Debug.LogError($"{nameof(SpriteChooser)} requires a {nameof(SpriteRenderer)} component");
            }
        }
    }
}