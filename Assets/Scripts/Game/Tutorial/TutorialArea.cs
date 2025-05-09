using System.Collections;
using Tools;
using UnityEngine;

namespace Game.Tutorial {
    public class TutorialArea : MonoBehaviour {
        [SerializeField] private float timeToAppear = 0;
        [SerializeField] private float disappearTime = 100000;
        [TextArea(3, 10)]
        [SerializeField] private string text;
        [SerializeField] private bool playOnce = true;
        
        private bool _hasPlayed;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(Tags.Child)) return;
            if (playOnce && _hasPlayed) {
                return;
            }
            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer() {
            yield return new WaitForSeconds(timeToAppear);
            TutorialPopupObject.ShowTutorialPopup(text);
            _hasPlayed = true;
            yield return new WaitForSeconds(disappearTime);
            TutorialPopupObject.HideTutorialPopup();
        }

        private void OnTriggerExit2D(Collider2D other) {
            StopAllCoroutines();
            TutorialPopupObject.HideTutorialPopup();
        }
    }
}