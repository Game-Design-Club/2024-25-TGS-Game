using System.Collections;
using UnityEngine;

namespace DefaultNamespace {
    public class Stone : MonoBehaviour {
        internal bool inWater = false;
        [SerializeField] private float timeToSink = .3f;
        
        public void InWater(Vector2 target) {
            inWater = true;
            GetComponent<Collider2D>().isTrigger = true;
            StartCoroutine(MoveToPosition(target, timeToSink));
        }
        
        private IEnumerator MoveToPosition(Vector2 target, float time) {
            float elapsedTime = 0;
            while (elapsedTime < time) {
                transform.position = Vector2.Lerp(transform.position, target, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}