using Game;
using Game.GameManagement;

namespace AppCore.InputManagement {
    public class InputManager : AppModule {
        public PlayerInputs PlayerInputs;
        
        private void Awake() {
            PlayerInputs = new PlayerInputs();
        }
        
        private void OnEnable() {
            PlayerInputs.Enable();
            PlayerInputs.UI.Enable();
            
            GameManager.OnGameEvent += OnGameEvent;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
            
            PlayerInputs.Disable();
        }

        private void OnGameEvent(GameEvent gameEvent) {
            switch (gameEvent.GameEventType) {
                case GameEventType.Bear:
                    PlayerInputs.Bear.Enable();
                    PlayerInputs.Child.Disable();
                    break;
                case GameEventType.Child:
                    PlayerInputs.Bear.Disable();
                    PlayerInputs.Child.Enable();
                    break;
                case GameEventType.Cutscene:
                    PlayerInputs.Bear.Disable();
                    PlayerInputs.Child.Disable();
                    break;
                default:
                    break;
            }
        }
    }
}