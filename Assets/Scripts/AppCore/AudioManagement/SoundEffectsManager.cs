using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace AppCore.AudioManagement {
    public class SoundEffectsManager : MonoBehaviour {
        [SerializeField] private AudioMixerGroup sfxGroup;
        
        private Dictionary<SoundEffect, AudioSource> _audioSources = new();

        public void PlaySoundEffect(SoundEffect soundEffect) {
            if (soundEffect == null) {
                Debug.LogWarning("SoundEffect is null");
                return;
            }
            GameObject soundObject = new GameObject("SFX");
            if (soundEffect.position != null) 
                soundObject.transform.position = (Vector2) soundEffect.position;
            soundObject.transform.SetParent(soundEffect.parent == null ? transform : soundEffect.parent);
            AudioSource source = soundObject.AddComponent<AudioSource>();
            
            _audioSources[soundEffect] = source;

            source.clip = soundEffect.clips.Random();
            float targetVolume = soundEffect.volume; // desired volume level

            // Start at 0 if fading in; otherwise, start at full volume.
            source.volume = (soundEffect.fadeInTime > 0) ? 0f : targetVolume;
            
            source.pitch = soundEffect.pitch + Random.Range(-soundEffect.pitchRandomness, soundEffect.pitchRandomness);
            source.spatialBlend = soundEffect.spatialBlend;
            source.minDistance = soundEffect.minDistance;
            source.maxDistance = soundEffect.maxDistance;
            source.outputAudioMixerGroup = sfxGroup;
            source.loop = soundEffect.loop;
            source.Play();
            
            StartCoroutine(PlaySourceAndRemove(soundEffect, source, targetVolume));
        }

        private IEnumerator PlaySourceAndRemove(SoundEffect soundEffect, AudioSource soundSource, float targetVolume) {
            float currentVolume = soundSource.volume;
            Func<bool> continueCondition = soundEffect.continueCondition;
            continueCondition ??= () => soundSource != null && soundSource.isPlaying;
            
            while (true) {
                
                bool isActive = continueCondition();
                if (!isActive && Mathf.Approximately(currentVolume, 0f)) {
                    break;
                }

                if (soundEffect == null) {
                    Debug.LogWarning("SoundEffect is null, ");
                }
                float desiredVolume = isActive && !soundEffect.paused() ? targetVolume : 0f;

                float fadeRate = 0f;
                if (desiredVolume > currentVolume) {
                    fadeRate = (soundEffect.fadeInTime > 0) ? (targetVolume / soundEffect.fadeInTime) : float.MaxValue;
                } else if (desiredVolume < currentVolume) {
                    fadeRate = (soundEffect.fadeOutTime > 0) ? (targetVolume / soundEffect.fadeOutTime) : float.MaxValue;
                }
                currentVolume = Mathf.MoveTowards(currentVolume, desiredVolume, Time.deltaTime * fadeRate);
                soundSource.volume = currentVolume;
                
                
                yield return null;
            }
            
            Destroy(soundSource.gameObject);
            _audioSources.Remove(soundEffect);
        }

        public void StopSoundEffect(SoundEffect soundEffect) {
            if (_audioSources.ContainsKey(soundEffect)) {
                soundEffect.paused = () => true;
                _audioSources.Remove(soundEffect);
            } else {
                // Debug.LogWarning("SoundEffect not found in _audioSources");
            }
        }
    }
}