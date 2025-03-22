using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AppCore.DataManagement
{
    public class DataManager : AppModule {
        [SerializeField] private int defaultSaveFile = 0;
        private string _saveFilePath;
        private Dictionary<string, bool> _boolFlags;
        public List<string> FoundScrapbookItems { get; private set; }
        public Vector3 PlayerPosition { get; private set; }

        private void Awake() {
            if (_saveFilePath == null) {
                LoadData(defaultSaveFile);
            }
        }

        public void LoadData(int fileNumber)
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, $"savedata{fileNumber}.json");
            Load();
        }
        
        // [ContextMenu("Reset Data")]
        public void ResetData() {
            for (int i = 0; i < 10; i++) {
                _boolFlags = new();
                FoundScrapbookItems = new();
                PlayerPosition = Vector3.zero;
                _saveFilePath = Path.Combine(Application.persistentDataPath, $"savedata{i}.json");
                Save();
            }
        }

        public bool GetFlag(string key) {
            if (_boolFlags.TryGetValue(key, out var result)) return result;
            
            // Debug.LogWarning("Flag not found: " + key);
            return false;
        }

        public void SetFlag(string key, bool value) {
            _boolFlags[key] = value;
        }

        public void AddScrapbookItem(string item)
        {
            if (!FoundScrapbookItems.Contains(item))
            {
                FoundScrapbookItems.Add(item);
            }
        }

        public bool HasScrapbookItem(string item)
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
            foreach (var pair in _boolFlags)
            {
                data.boolFlags.Add(new BoolFlag { key = pair.Key, value = pair.Value });
            }

            data.foundScrapbookItems = FoundScrapbookItems;
            data.playerPosition = PlayerPosition;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_saveFilePath, json);
            
            // Debug.Log("Saved file: " + _saveFilePath + ", " + json, gameObject);
        }

        private void Load()
        {
            if (File.Exists(_saveFilePath))
            {
                string json = File.ReadAllText(_saveFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                _boolFlags = new Dictionary<string, bool>();
                foreach (BoolFlag flag in data.boolFlags)
                {
                    _boolFlags[flag.key] = flag.value;
                }

                FoundScrapbookItems = data.foundScrapbookItems;
                PlayerPosition = data.playerPosition;
            }
            else
            {
                Debug.Log("No save file found, starting a new one.");
                ResetData();
            }
        }

        private void OnApplicationQuit() {
            Save();
        }

        private void Update() {
            // Debug.Log(
                // $"Player position: {PlayerPosition}, " + 
                // $"Found scrapbook items: {FoundScrapbookItems.Count}, " +
                // $"Bool flags: {_boolFlags.Count}"
            // );
        }

        public bool FlagExists(string flagName) {
            return _boolFlags.ContainsKey(flagName);
        }

        public bool TryGetFlag(out bool value, string flagName) {
            return _boolFlags.TryGetValue(flagName, out value);
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
        public List<string> foundScrapbookItems;
        public Vector3 playerPosition;
    }
}