using AppCore;
using AppCore.DataManagement;
using AppCore.SceneManagement;
using Tools;
using UnityEngine;

namespace MainMenu.Management {
    public class MainMenuManager : MonoBehaviour {
        public void LoadLevel(int fileIndex) {
            App.Get<DataManager>().LoadData(fileIndex);
            App.Get<SceneLoader>().LoadScene(Scenes.Game);
        }

        public void ResetData() {
            App.Get<DataManager>().ResetData();
        }
    }
}