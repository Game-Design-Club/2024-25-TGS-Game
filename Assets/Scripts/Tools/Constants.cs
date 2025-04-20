namespace Tools {
    // public static class Constants {
        public static class AnimationConstants {
            public static class GameUI {
                public const string IsPaused = "IsPaused";
                public const string IsGameOver = "IsGameOver";
            }

            public static class Bear {
                public const string Swipe = "Swipe";
                public const string Growl = "Growl Execute";
                public const string GrowlChargeup = "Growl Charge";
                public const string Idle = "Idle";
                public const string Stun = "Stun";
                public const string Pounce = "Pounce";
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

            public class ShootEnemy {
                public const string Teleport = "Teleport";
            }

            public class CombatArea {
                public const string EnterCombat = "To Combat";
            }

            public class Avalanche {
                public const string Start = "Start";
                public static string Reset = "Reset";
            }
        }

        public static class Tags {
            public const string Enemy = "Enemy";
            public const string Child = "Child";
            public const string Bear = "Bear";
            public const string BearEnemyDamageable = "Bear Enemy Hittable";
            public const string EnemyDestroyer = "EnemyDestroyer";
            public const string River = "River";
            public const string Ground = "Ground";
            public const string Rock = "Rock";
            public const string Log = "Log";
            public const string RiverBase = "River Base";
            public const string Avalanche = "Avalanche";
        }

        public static class PhysicsLayers {
            public const int ChildWall = 8;
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