using AppCore;
using AppCore.AudioManagement;
using AppCore.DataManagement;
using AppCore.SceneManagement;
using MainMenu.Credits;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.Management {
    public class MainMenuManager : MonoBehaviour {
        [SerializeField] private Animator menuAnimator;
        [SerializeField] private GameObject buttonBlocker;
        [Header("Options References")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [Header("Credits References")]
        [SerializeField] private CreditsScroller credits;

        private void Awake() {
            Time.timeScale = 1;

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

        public void PlayButton() {
            menuAnimator.SetBool(AnimationParameters.MainMenu.Play, true);
        }
        public void UnPlayButton() {
            menuAnimator.SetBool(AnimationParameters.MainMenu.Play, false);
        }
        public void OptionsButton() {
            menuAnimator.SetBool(AnimationParameters.MainMenu.Options, true);
        }
        public void UnOptionsButton() {
            menuAnimator.SetBool(AnimationParameters.MainMenu.Options, false);
        }
        
        public void CreditsButton() {
            menuAnimator.SetBool(AnimationParameters.MainMenu.Credits, true);
            credits.StartCredits();
        }
        public void UnCreditsButton() {
            menuAnimator.SetBool(AnimationParameters.MainMenu.Credits, false);
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

        private void OnDestroy() {
            App.Get<DataManager>().SavePreferences();
        }

        public void Quit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        public void LoadLevel(int fileIndex) {
            App.Get<DataManager>().LoadFile(fileIndex);
            App.Get<SceneLoader>().LoadScene(Scenes.Game);
        }

        public void ResetData() {
            App.Get<DataManager>().ResetData();
        }

        public void EraseFile(int fileNumber) {
            App.Get<DataManager>().EraseFile(fileNumber);
        }

        private void Update() {
            
        }
    }
}