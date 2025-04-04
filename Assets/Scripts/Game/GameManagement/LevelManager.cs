using UnityEngine;

namespace Game.GameManagement {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private Level currentLevel;
        
        public static LevelManager Instance { get; private set; }
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
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
        
        public static Level GetCurrentLevel() {
            if (Instance == null) {
                Debug.LogError("LevelManager instance is null. Ensure it is initialized before calling GetCurrentLevel.");
                return null;
            }
            return Instance.currentLevel;
        }
    }
}