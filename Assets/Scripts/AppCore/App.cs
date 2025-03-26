using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppCore {
    public class App : MonoBehaviour {
        // Singleton class for the app
        // Contains references to all managers that can be used in different scenes in the game
        // (for example main menu, game, even credits, etc.)
        // Consolidates singleton patterns into one place so there aren't instance checks in every script
        
        private static App _instance;

        [SerializeField] private List<AppModule> appModules;
        
        private readonly Dictionary<Type, AppModule> _moduleDictionary = new();
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static T Get<T>() where T : AppModule {
            if (_instance._moduleDictionary.TryGetValue(typeof(T), out AppModule module)) {
                return (T) module;
            }

            Debug.LogError($"No instance of {typeof(T)} found in App.");
            return null;
        }

        private void Awake() {
            // Sets up singleton pattern
            if (_instance == null) {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
                return;
            }
            
            CheckAppModules();
        }

        private void CheckAppModules() {
            foreach (AppModule appModule in appModules) {
                Type moduleType = appModule.GetType();
                if (!_moduleDictionary.TryAdd(moduleType, appModule)) {
                    Debug.LogError($"Multiple instances of {moduleType} found in App.");
                }
            }
        }
        
        // Used to check if the App instance is in the scene, otherwise throw an error
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CheckAppInstance() {
            if (_instance == null) {
                Debug.LogError("No App instance found in the scene.");
            }
        }
    }
}
