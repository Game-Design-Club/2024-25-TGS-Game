using System;
using AppCore;
using AppCore.AudioManagement;
using AppCore.DataManagement;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.Menu {
    public class OptionsMenu : MonoBehaviour {
        [Header("Options References")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        private void Awake() {
            SetVolumes();
        }
        
        private void SetVolumes() {
            float master = App.Get<DataManager>().GetMasterVolume();
            float music = App.Get<DataManager>().GetMusicVolume();
            float sfx = App.Get<DataManager>().GetSFXVolume();
            
            masterVolumeSlider.value = master;
            musicVolumeSlider.value = music;
            sfxVolumeSlider.value = sfx;
            
            App.Get<AudioManager>().SetMasterVolume(master);
            App.Get<AudioManager>().SetMusicVolume(music);
            App.Get<AudioManager>().SetSFXVolume(sfx);
        }

        public void SetMasterVolume(float value) {
            App.Get<DataManager>().SetMasterVolume(value);
            App.Get<AudioManager>().SetMasterVolume(value);
        }
        
        public void SetMusicVolume(float value) {
            App.Get<DataManager>().SetMusicVolume(value);
            App.Get<AudioManager>().SetMusicVolume(value);
        }
        
        public void SetSFXVolume(float value) {
            App.Get<DataManager>().SetSFXVolume(value);
            App.Get<AudioManager>().SetSFXVolume(value);
        }
    }
}