namespace Tools {
    public static class Constants {
        public static class Animator {
            public static class GameUI {
                public const string IsPaused = "IsPaused";
            }

            public static class Bear {
                public const string Swipe = "Swipe";
                public const string Growl = "Growl Execute";
                public const string GrowlChargeup = "Growl Charge";
                public const string Idle = "Idle";
                public static string Stun = "Stun";
            }

            public static class BearIDs {
                public const int Swipe = 1;
                public const int Growl = 2;
                public const int GrowlChargeup = 3;
                public const int Stun = 4;
            }
            public static class Child {
                public const string Sleep = "Sleep";
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
        }

        public static class Tags {
            public const string Enemy = "Enemy";
            public const string Child = "Child";
            public static string Bear = "Bear";
        }

        public static class Layers {
            public const string ChildGameplay = "Child Gameplay";
            public const string ChildGameplayFront = "Child Gameplay Front";
        }
    }
}