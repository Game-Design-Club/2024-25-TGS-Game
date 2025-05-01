using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace AppCore.DataManagement
{
    public class DataManager : AppModule {
        [SerializeField] private bool printSave = false;
        [SerializeField] private int defaultSaveFile = 0;
        
        
        private string _prefsFilePath;
        
        // Data
        private Dictionary<string, bool> _boolFlags;
        private Dictionary<string, object> _customData;
        public List<string> FoundScrapbookItems { get; private set; }
        public Vector3 PlayerPosition { get; private set; }
        [FormerlySerializedAs("setPlayerPosition")] public bool firstLevelLoad = true;
        private float _masterVolume = 1f;
        private float _musicVolume = 1f;
        private float _sfxVolume = 1f;
        
        private int _currentLoadedFile = -1;
        public bool IsFileLoaded => _currentLoadedFile != -1;
        

        private void Awake() {
            // Load or create the active save‑slot data
            if (_currentLoadedFile != -1) {
                LoadFile(defaultSaveFile);
            }

            // Set up and load player‑preference data
            _prefsFilePath = Path.Combine(Application.persistentDataPath, "preferences.json");
            LoadPreferences();
        }
        
        [ContextMenu("Reset Data")]
        public void ResetData() {
            for (int i = 0; i < 3; i++) {
                ResetFile(i);
            }
        }

        [ContextMenu("Reset File")]
        private void ResetFile(int i) {
            _boolFlags = new();
            _customData = new();
            FoundScrapbookItems = new();
            PlayerPosition = Vector3.zero;
            firstLevelLoad = true;
            SaveFile(i);
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

        public void SetMasterVolume(float value) {
            _masterVolume = value;
            SavePreferences();
        }
        
        public float GetMasterVolume() {
            return _masterVolume;
        }
        
        public void SetMusicVolume(float value) {
            _musicVolume = value;
            SavePreferences();
        }
        
        public float GetMusicVolume() {
            return _musicVolume;
        }
        
        public void SetSFXVolume(float value) {
            _sfxVolume = value;
            SavePreferences();
        }
        public float GetSFXVolume() {
            return _sfxVolume;
        }
        
        public void SaveFile(int index) {
            if (!IsFileLoaded) return;
            
            string saveFilePath = Path.Combine(Application.persistentDataPath, $"savedata{index}.json");
            
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
            File.WriteAllText(saveFilePath, json);
            
            if (printSave) Debug.Log("Saved file: " + saveFilePath + ", " + json, gameObject);
        }

        public void LoadFile(int index)
        {
            _currentLoadedFile = index;
            string saveFilePath = Path.Combine(Application.persistentDataPath, $"savedata{index}.json");

            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
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
                ResetFile(index);
            }
        }

        private void LoadPreferences() {
            if (File.Exists(_prefsFilePath)) {
                string json = File.ReadAllText(_prefsFilePath);
                PrefsData prefs = JsonUtility.FromJson<PrefsData>(json);
                _masterVolume = prefs.masterVolume;
                _musicVolume  = prefs.musicVolume;
                _sfxVolume    = prefs.sfxVolume;
            } else {
                SavePreferences();
            }
        }

        public void SavePreferences() {
            PrefsData prefs = new PrefsData {
                masterVolume = _masterVolume,
                musicVolume  = _musicVolume,
                sfxVolume    = _sfxVolume
            };

            string json = JsonUtility.ToJson(prefs, true);
            File.WriteAllText(_prefsFilePath, json);
            if (printSave) Debug.Log("Saved preferences: " + json, gameObject);
        }

        private void OnApplicationQuit() {
            if (IsFileLoaded) {
                SaveFile(_currentLoadedFile);
            }
            SavePreferences();
            _currentLoadedFile = -1;
        }

        private void Update() {
            // Debug.Log(
                // $"Player position: {PlayerPosition}, " + 
                // $"Found scrapbook items: {FoundScrapbookItems.Count}, " +
                // $"Bool flags: {_boolFlags.Count}"
            // );
        }

        public void EraseFile(int fileNumber) {
            ResetFile(fileNumber);
        }

        public bool IsLevelCreated(int fileNumber) {
            string filePath = Path.Combine(Application.persistentDataPath, $"savedata{fileNumber}.json");
            return File.Exists(filePath) && !JsonUtility.FromJson<SaveData>(File.ReadAllText(filePath)).firstLevelLoad;
        }

        public void SaveCurrentFile() {
            if (_currentLoadedFile != -1) {
                SaveFile(_currentLoadedFile);
            } else {
                Debug.LogWarning("No file loaded to save.");
            }
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
        public float masterVolume = 1f;
        public float musicVolume = 1f;
        public float sfxVolume = 1f;
    }

    [System.Serializable]
    public class PrefsData
    {
        public float masterVolume = 1f;
        public float musicVolume  = 1f;
        public float sfxVolume    = 1f;
    }
}