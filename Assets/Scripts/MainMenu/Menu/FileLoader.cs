using System;
using AppCore;
using AppCore.DataManagement;
using MainMenu.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.Menu {
    public class FileLoader : MonoBehaviour {
        [SerializeField] private MainMenuManager mainMenuManager;
        [SerializeField] private int fileNumber;
        [SerializeField] private Image indicatorImage;
        [SerializeField] private Color loadedColor = Color.green;
        [SerializeField] private Color notLoadedColor = Color.yellow;

        private void Start() {
            if (App.Get<DataManager>().IsLevelCreated(fileNumber)) {
                indicatorImage.color = loadedColor;
                Debug.Log("Level " + fileNumber + " exists");
            } else {
                indicatorImage.color = notLoadedColor;
                Debug.Log("Level " + fileNumber + " doesn't exist");
            }
        }

        public void LoadFile() {
            mainMenuManager.LoadLevel(fileNumber);
        }
        
        public void EraseFile() {
            mainMenuManager.EraseFile(fileNumber);
            indicatorImage.color = notLoadedColor;
        }
    }
}