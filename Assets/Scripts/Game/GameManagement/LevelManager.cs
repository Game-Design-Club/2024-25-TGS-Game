using UnityEngine;

namespace Game.GameManagement {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private Level currentLevel;
        
        public static LevelManager Instance { get; private set; }
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                if (currentLevel == null) {
                    Debug.LogWarning("Current level is not set. Please assign a level in the inspector.");
                    currentLevel = FindAnyObjectByType<Level>();
                }
            } else {
                Debug.LogWarning("Multiple instances of LevelManager detected. Destroying the new instance.");
                Destroy(gameObject);
            }
        }
        
        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static Level GetCurrentLevel() {
            if (Instance) return Instance.currentLevel;
            
            Debug.LogError("LevelManager instance is null. Ensure it is initialized before calling GetCurrentLevel.");
            return null;
        }
    }
}