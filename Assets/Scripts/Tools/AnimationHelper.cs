using System.Collections;
using Tools.Extensions;
using UnityEngine;

namespace Tools {
    public static class AnimationHelper {
        public static IEnumerator MoveToPosition(this MonoBehaviour component, Transform transform, Vector3 targetPosition, AnimationCurve curve) {
            yield return component.StartCoroutine(MoveToPositionCoroutine(transform, targetPosition, curve));
        }
        
        public static IEnumerator MoveToPosition(this MonoBehaviour component, Rigidbody2D rigidbody, Vector2 targetPosition, AnimationCurve curve) {
            yield return component.StartCoroutine(MoveToPositionCoroutine(rigidbody, targetPosition, curve));
        }
        
        private static IEnumerator MoveToPositionCoroutine(Rigidbody2D rigidbody, Vector2 targetPosition, AnimationCurve curve) {
            float t = 0;
            Vector2 startPosition = rigidbody.position;
            while (t < curve.Time()) {
                t += Time.deltaTime;
                rigidbody.position = Vector2.Lerp(startPosition, targetPosition, curve.Evaluate(t));
                yield return null;
            }
        }
        
        private static IEnumerator MoveToPositionCoroutine(Transform transform, Vector3 targetPosition, AnimationCurve curve) {
            float t = 0;
            Vector3 startPosition = transform.position;
            while (t < curve.Time()) {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(targetPosition, startPosition, curve.Evaluate(t));
                yield return null;
            }
        }
    }
}