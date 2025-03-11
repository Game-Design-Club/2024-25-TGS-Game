using UnityEngine;

namespace Tools.LevelDesign {
    public class SpriteChooser : MonoBehaviour {
        [SerializeField] private int objectNumber;
        [SerializeField] private SpriteChooserData _spriteChoserData;

        private void OnValidate() {
            if (objectNumber >= _spriteChoserData.objects.Length) {
                objectNumber = _spriteChoserData.objects.Length - 1;
            }
            if (objectNumber < 0) {
                objectNumber = 0;
            }
            if (TryGetComponent(out SpriteRenderer spriteRenderer)) {
                spriteRenderer.sprite = _spriteChoserData.objects[objectNumber];
            } else {
                Debug.LogError($"{nameof(SpriteChooser)} requires a {nameof(SpriteRenderer)} component");
            }
        }
    }
}