namespace Tools {
    // public static class Constants {
        public static class AnimationConstants {
            public static class GameUI {
                public const string IsPaused = "IsPaused";
                public static string IsGameOver = "IsGameOver";
            }

            public static class Bear {
                public const string Swipe = "Swipe";
                public const string Growl = "Growl Execute";
                public const string GrowlChargeup = "Growl Charge";
                public const string Idle = "Idle";
                public static string Stun = "Stun";
                public static string Pounce = "Pounce";
            }

            public static class BearIDs {
                public const int Swipe = 1;
                public const int Growl = 2;
                public const int GrowlChargeup = 3;
                public const int Stun = 4;
                public static int Pounce = 5;
            }
            public static class Child {
                public const string Sleep = "Sleep";
                public static string Attack = "Attack";
                public static string Jump = "Jump";
                public static string Float = "Float";
            }

            public static class DialogueBox {
                public const string IsOpen = "Open";
            }

            public static class AttackEnemy {
                public const string Attack = "Attack";
                public const string Idle = "Idle";
            }
            
            public static class TreeEnemy {
                public const string Attack = "Attack";
                public const string Reset = "Reset";
            }

            public static class Transitions {
                public const string FadeOut = "Fade Out";
                public const string FadeIn = "Fade In";
            }
        }

        public static class Tags {
            public const string Enemy = "Enemy";
            public const string Child = "Child";
            public static string Bear = "Bear";
            public static string BearEnemyDamageable = "Bear Enemy Hittable";
            public static string EnemyDestroyer = "EnemyDestroyer";
            public static string River = "River";
            public static string Ground = "Ground";
            public static string Rock = "Rock";
            public static string Log = "Log";
            public static string RiverBase = "River Base";
        }

        public static class Layers {
            public const string ChildGameplay = "Child Gameplay";
            public const string ChildGameplayFront = "Child Gameplay Front";
        }

        public static class Mixer {
            public const string MasterVolume = "Master Volume";
            public const string SFXVolume = "SFX Volume";
            public const string MusicVolume = "Music Volume";
        }

        public static class BoolFlags {
            public const string HasStick = "HasStick";
        }

        public static class Scenes {
            public const int MainMenu = 0;
            public const int Game = 1;
        }
    // }
}