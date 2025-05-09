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
        [SerializeField] private Music mainMenuMusic;
        [Header("Credits References")]
        [SerializeField] private CreditsScroller credits;

        private void Awake() {
            Time.timeScale = 1;
            App.Get<MusicManager>().PlayMusic(mainMenuMusic);
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
    }
}