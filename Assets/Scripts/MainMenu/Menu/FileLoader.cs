using MainMenu.Management;
using TMPro;
using UnityEngine;

namespace MainMenu.Menu {
    public class FileLoader : MonoBehaviour {
        [SerializeField] private MainMenuManager mainMenuManager;
        [SerializeField] private int fileNumber;
        
        public void LoadFile() {
            mainMenuManager.LoadLevel(fileNumber);
        }
        
        public void EraseFile() {
            mainMenuManager.EraseFile(fileNumber);
        }
    }
}