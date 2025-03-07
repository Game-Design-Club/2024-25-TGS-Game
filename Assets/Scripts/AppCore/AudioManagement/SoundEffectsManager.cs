using Tools;
using UnityEngine;
using UnityEngine.Audio;

namespace AppCore.AudioManagement {
    public class SoundEffectsManager : MonoBehaviour {
        [SerializeField] private AudioMixerGroup sfxGroup;
        
        public void PlaySoundEffect(SoundEffect soundEffect) {
            if (soundEffect == null) {
                Debug.LogWarning("SoundEffect is null");
                return;
            }
            GameObject soundObject = new GameObject("SFX");
            if (soundEffect.position != null) soundObject.transform.position = (Vector2) soundEffect.position;
            soundObject.transform.SetParent(soundEffect.parent == null ? transform : soundEffect.parent);
            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.clip = soundEffect.clips.Random();
            source.volume += soundEffect.volume;
            source.pitch += soundEffect.pitch + Random.Range(-soundEffect.pitchRandomness, soundEffect.pitchRandomness);
            source.spatialBlend = soundEffect.spatialBlend;
            source.minDistance = soundEffect.minDistance;
            source.maxDistance = soundEffect.maxDistance;
            source.outputAudioMixerGroup = sfxGroup;
            source.loop = soundEffect.loop;
            source.Play();
        }
    }
}