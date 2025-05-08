using System.Collections;
using UnityEngine;

namespace Game.Tutorial {
    public class TutorialArea : MonoBehaviour {
        [SerializeField] private float timeToAppear = 0;
        [TextArea(3, 10)]
        [SerializeField] private string text;
        private void OnTriggerEnter2D(Collider2D other) {
            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer() {
            yield return new WaitForSeconds(timeToAppear);
            TutorialPopupObject.ShowTutorialPopup(text);
        }

        private void OnTriggerExit2D(Collider2D other) {
            StopAllCoroutines();
            TutorialPopupObject.HideTutorialPopup();
        }
    }
}