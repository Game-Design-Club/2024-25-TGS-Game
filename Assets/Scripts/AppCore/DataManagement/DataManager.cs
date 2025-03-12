using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Game.Exploration.Enviornment.Interactables.Scrapbook;

namespace AppCore.DataManagement
{
    public class DataManager : AppModule
    {
        private string saveFilePath;
        private Dictionary<string, bool> boolFlags = new();
        public List<ScrapbookItem> FoundScrapbookItems { get; private set; } = new();
        public Vector3 PlayerPosition { get; private set; } = Vector3.zero;

        private void Awake() {
            saveFilePath = Application.persistentDataPath;
        }

        public void LoadData(int fileNumber)
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, $"savedata{fileNumber}.json");
            Load();
        }
        
        [ContextMenu("Reset Data")]
        public void ResetData() {
            boolFlags.Clear();
            FoundScrapbookItems.Clear();
            PlayerPosition = Vector3.zero;
        }

        public bool GetFlag(string key) {
            if (boolFlags.TryGetValue(key, out var result)) return result;
            
            Debug.LogWarning("Flag not found: " + key);
            return false;
        }

        public void SetFlag(string key, bool value) {
            boolFlags[key] = value;
        }

        public void AddScrapbookItem(ScrapbookItem item)
        {
            if (!FoundScrapbookItems.Contains(item))
            {
                FoundScrapbookItems.Add(item);
            }
        }

        public bool HasScrapbookItem(ScrapbookItem item)
        {
            return FoundScrapbookItems.Contains(item);
        }

        public void UpdatePlayerPosition(Vector3 newPosition)
        {
            PlayerPosition = newPosition;
        }

        public void Save() {
            SaveData data = new SaveData();
            
            data.boolFlags = new List<BoolFlag>();
            foreach (var pair in boolFlags)
            {
                data.boolFlags.Add(new BoolFlag { key = pair.Key, value = pair.Value });
            }

            data.foundScrapbookItems = FoundScrapbookItems;
            data.playerPosition = PlayerPosition;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game data saved");
        }

        private void Load()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                boolFlags = new Dictionary<string, bool>();
                foreach (BoolFlag flag in data.boolFlags)
                {
                    boolFlags[flag.key] = flag.value;
                }

                FoundScrapbookItems = data.foundScrapbookItems;
                PlayerPosition = data.playerPosition;
            }
            else
            {
                Debug.Log("No save file found, starting a new one.");
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        private void Update() {
            Debug.Log(
                $"Player position: {PlayerPosition}, " + 
                $"Found scrapbook items: {FoundScrapbookItems.Count}, " +
                $"Bool flags: {boolFlags.Count}"
            );
        }
    }

    [System.Serializable]
    public class BoolFlag
    {
        public string key;
        public bool value;
    }

    [System.Serializable]
    public class SaveData
    {
        public List<BoolFlag> boolFlags;
        public List<ScrapbookItem> foundScrapbookItems;
        public Vector3 playerPosition;
    }
}