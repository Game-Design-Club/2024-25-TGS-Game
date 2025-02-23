using AppCore.DialogueManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables.Scrapbook
{
    [CreateAssetMenu(fileName = "New Scrapbook Page", menuName = "Scrapbook/Page")]
    public class ScrapbookPage : ScriptableObject
    {
        [SerializeField] public string title;
        [SerializeField] public ScrapbookItemAndPos[] items;

        [System.Serializable]
        public class ScrapbookItemAndPos
        {
            [SerializeField] public ScrapbookItem item;
            [SerializeField] public Vector2 pos;
        }
    }
}