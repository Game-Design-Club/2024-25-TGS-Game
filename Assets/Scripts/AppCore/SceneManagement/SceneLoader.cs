using System.Collections;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore.SceneManagement {
    public class SceneLoader : AppModule {
        private bool _loading = false;
        [SerializeField] private Animator transitionAnimator;
        [SerializeField] private float transitionDuration = 1f;
        
        public void LoadScene(int sceneIndex) {
            if (_loading) return;
            _loading = true;
            StartCoroutine(LoadSceneCoroutine(sceneIndex));
        }

        private IEnumerator LoadSceneCoroutine(int sceneIndex) {
            transitionAnimator.SetTrigger(AnimationParameters.Transitions.FadeOut);
            yield return new WaitForSecondsRealtime(transitionDuration);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
            yield return asyncOperation;
            transitionAnimator.SetTrigger(AnimationParameters.Transitions.FadeIn);
            _loading = false;
        }
    }
}