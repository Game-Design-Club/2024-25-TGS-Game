using UnityEngine;

namespace MainMenu.Credits {
    [System.Serializable]
    public class CreditsSection { // Stores the data for a section of credits, passed to the CreditsAsset
        [SerializeField] public string title;
        [SerializeField] public string[] names;
    }
}