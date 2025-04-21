using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.CameraShaking {
    public class SpriteShaker : MonoBehaviour {
        [SerializeField] private float shakeIntensity = 0.1f;
        [SerializeField] private FloatRange shakeSwitchTime = new FloatRange(.1f, .4f);
        [SerializeField] private bool shakeOnAwake = true;

        private float _pivotX;
        private float _pivotY;
        
        private void Awake() {
            _pivotX = transform.localPosition.x; 
            _pivotY = transform.localPosition.y;

            if (shakeOnAwake) {
                StartCoroutine(WobbleCoroutine());
            }
        }

        private IEnumerator WobbleCoroutine() {
            while (true) {
                // Choose a random direction offset
                Vector2 targetOffset = new Vector2(
                    shakeIntensity.GetRandom(),
                    shakeIntensity.GetRandom()
                );
                float duration = shakeSwitchTime.Random();
                float elapsed = 0f;
                Vector2 startOffset = new Vector2(transform.localPosition.x - _pivotX, transform.localPosition.y - _pivotY);
                // Smoothly interpolate between start and target
                while (elapsed < duration) {
                    float t = elapsed / duration;
                    Vector2 currentOffset = Vector2.Lerp(startOffset, targetOffset, t);
                    transform.localPosition = new Vector2(_pivotX + currentOffset.x, _pivotY + currentOffset.y);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}