using UnityEngine;

namespace AppCore.FreezeFrameManagement {
    public class FreezeFrameManager : AppModule {
        [SerializeField] private float defaultDuration = .15f;
        [SerializeField] private float defaultTimeScale = .15f;
        private int _queue = 0;

        private Coroutine _freezeRoutine;

        public void FreezeFrame() {
            FreezeFrame(defaultDuration, defaultTimeScale);
        }

        public void FreezeFrame(float duration, float timeScale) {
            _queue++;
            _freezeRoutine ??= StartCoroutine(Freeze(duration, timeScale));
        }

        private System.Collections.IEnumerator Freeze(float duration, float timeScale) {
            Time.timeScale = timeScale;
            while (_queue > 0) {
                yield return new WaitForSecondsRealtime(duration);
                _queue--;
            }
            Time.timeScale = 1;
            _freezeRoutine = null;
        }
    }
}