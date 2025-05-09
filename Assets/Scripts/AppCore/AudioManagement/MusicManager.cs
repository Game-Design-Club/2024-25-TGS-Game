using System;
using System.Collections;
using Game.GameManagement;
using UnityEngine;

namespace AppCore.AudioManagement {
    public class MusicManager : AppModule {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float transitionTime = 1f;
        [SerializeField] private Music introMusic;
        [SerializeField] private Music mainMusic;
        [SerializeField] private Music combatMusic;

        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
        }

        private void OnGameEvent(GameEvent obj) {
            if (obj.GameEventType == GameEventType.Explore) {
                PlayMusic(mainMusic);
            } else if (obj.GameEventType == GameEventType.Combat) {
                PlayMusic(combatMusic);
            }
        }

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