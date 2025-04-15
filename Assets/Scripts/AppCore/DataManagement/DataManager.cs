using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace AppCore.DataManagement
{
    public class DataManager : AppModule {
        [SerializeField] private bool printSave = false;
        [SerializeField] private int defaultSaveFile = 0;
        private string _saveFilePath;
        private Dictionary<string, bool> _boolFlags;
        private Dictionary<string, object> _customData;
        public List<string> FoundScrapbookItems { get; private set; }
        public Vector3 PlayerPosition { get; private set; }
        [FormerlySerializedAs("setPlayerPosition")] public bool firstLevelLoad = true;

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
            for (int i = 0; i < 3; i++) {
                _boolFlags = new();
                _customData = new();
                FoundScrapbookItems = new();
                PlayerPosition = Vector3.zero;
                _saveFilePath = Path.Combine(Application.persistentDataPath, $"savedata{i}.json");
                firstLevelLoad = true;
                Save();
            }
        }

        public bool GetFlag(string key) {
            if (_boolFlags.TryGetValue(key, out var result)) return result;
            
            Debug.LogWarning("Flag not found: " + key);
            return false;
        }

        public void SetFlag(string key, bool value) {
            _boolFlags[key] = value;
        }
        
        public bool DoesFlagExist(string flagName) {
            return _boolFlags.ContainsKey(flagName);
        }

        public bool TryGetFlag(out bool value, string flagName) {
            return _boolFlags.TryGetValue(flagName, out value);
        }
        
        public void SetCustomData(string key, object value) {
            _customData[key] = value;
        }
        
        public object GetCustomData(string key) {
            if (_customData != null && _customData.TryGetValue(key, out var value)) {
                return value;
            }
            return null;
        }
        
        public bool CustomDataExists(string key) {
            return _customData != null && _customData.ContainsKey(key);
        }
        
        public bool TryGetCustomData(string key, out object value) {
            if (_customData != null && _customData.TryGetValue(key, out value)) {
                return true;
            }
            value = null;
            return false;
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
            firstLevelLoad = false;
        }

        public void Save() {
            SaveData data = new SaveData();
            
            data.boolFlags = new List<BoolFlag>();
            foreach (var pair in _boolFlags)
            {
                data.boolFlags.Add(new BoolFlag { key = pair.Key, value = pair.Value });
            }
            
            data.customData = new List<CustomData>();
            foreach (var pair in _customData)
            {
                string jsonValue = JsonUtility.ToJson(pair.Value);
                string typeName = pair.Value.GetType().AssemblyQualifiedName;
                data.customData.Add(new CustomData { key = pair.Key, jsonValue = jsonValue, typeName = typeName });
            }

            data.foundScrapbookItems = FoundScrapbookItems;
            data.playerPosition = PlayerPosition;
            data.firstLevelLoad = firstLevelLoad;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_saveFilePath, json);
            
            if (printSave) Debug.Log("Saved file: " + _saveFilePath + ", " + json, gameObject);
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
                
                _customData = new Dictionary<string, object>();
                foreach (CustomData customData in data.customData)
                {
                    System.Type type = System.Type.GetType(customData.typeName);
                    object value = JsonUtility.FromJson(customData.jsonValue, type);
                    _customData[customData.key] = value;
                }

                FoundScrapbookItems = data.foundScrapbookItems;
                PlayerPosition = data.playerPosition;
                firstLevelLoad = data.firstLevelLoad;
                
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
    }

    [System.Serializable]
    public class BoolFlag
    {
        public string key;
        public bool value;
    }
    
    [System.Serializable]
    public class CustomData
    {
        public string key;
        public string jsonValue;
        public string typeName;
    }

    [System.Serializable]
    public class SaveData
    {
        public List<BoolFlag> boolFlags;
        public List<CustomData> customData;
        public List<string> foundScrapbookItems;
        public Vector3 playerPosition;
        public bool firstLevelLoad = true;
    }
}