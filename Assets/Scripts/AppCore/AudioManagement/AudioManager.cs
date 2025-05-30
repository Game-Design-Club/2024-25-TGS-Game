using Tools;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace AppCore.AudioManagement {
    public class AudioManager : AppModule {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private SoundEffectsManager soundEffectsManager;
        [Range(0f, 1f)] [SerializeField] public float masterVolume = 1;

        private void Awake() {
            mixer.SetFloat(Mixer.MasterVolume, ConvertToDecibels(masterVolume));
        }

        private void OnValidate() {
            mixer.SetFloat(Mixer.MasterVolume, ConvertToDecibels(masterVolume));
        }


        public void PlaySoundEffect(SoundEffect soundEffect) {
            soundEffectsManager.PlaySoundEffect(soundEffect);
        }
        
        internal static float ConvertToDecibels(float volume) {
            volume = Mathf.Clamp(volume, 0.0001f, 1f);
            return Mathf.Log10(volume) * 20;
        }

        public void StopSoundEffect(SoundEffect soundEffect) {
            soundEffectsManager.StopSoundEffect(soundEffect);
        }

        public void SetMasterVolume(float value) {
            masterVolume = value;
            mixer.SetFloat(Mixer.MasterVolume, ConvertToDecibels(masterVolume));
        }
        
        public void SetMusicVolume(float value) {
            mixer.SetFloat(Mixer.MusicVolume, ConvertToDecibels(value));
        }
        
        public void SetSFXVolume(float value) {
            mixer.SetFloat(Mixer.SFXVolume, ConvertToDecibels(value));
        }
    }
}
