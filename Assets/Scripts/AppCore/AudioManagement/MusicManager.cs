using System.Collections;
using UnityEngine;

namespace AppCore.AudioManagement {
    public class MusicManager : AppModule {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float transitionTime = 1f;

        public void PlayMusic(Music music) {
            StartCoroutine(PlayMusicCoroutine(music));
        }

        private IEnumerator PlayMusicCoroutine(Music music) {
            float t = 0f;
            float startVolume = audioSource.volume;
            float targetVolume = 0f;
            while (t < transitionTime) {
                t += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / transitionTime);
                yield return null;
            }
            
            audioSource.clip = music.audioClip;
            audioSource.Play();
            t = 0f;
            startVolume = audioSource.volume;
            targetVolume = music.volume;
            while (t < transitionTime) {
                t += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / transitionTime);
                yield return null;
            }
        }
    }
}