using AppCore.DialogueManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables.Scrapbook
{
    [CreateAssetMenu(fileName = "New Scrapbook Page", menuName = "Scrapbook/Page")]
    public class ScrapbookPage : ScriptableObject
    {
        [SerializeField] public string title;
        [SerializeField] public string description;
        [SerializeField] public ScrapbookItemUIInfo[] items;

        [System.Serializable]
        public class ScrapbookItemUIInfo
        {
            [SerializeField] public ScrapbookItem item;
            [SerializeField] public Vector2 pos;
            [SerializeField] public float size = 1;
        }
    }
}