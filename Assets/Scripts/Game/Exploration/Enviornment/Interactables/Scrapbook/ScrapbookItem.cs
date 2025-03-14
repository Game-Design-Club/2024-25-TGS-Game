using AppCore.DialogueManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Exploration.Enviornment.Interactables.Scrapbook
{
    [CreateAssetMenu(fileName = "New Scrapbook Item", menuName = "Scrapbook/Item")]
    public class ScrapbookItem : ScriptableObject
    {
        [FormerlySerializedAs("name")] [SerializeField] public string itemName;
        [SerializeField] public Sprite sprite;
        [SerializeField] public string description;

        public bool Equals(ScrapbookItem other)
        {
            return itemName.Equals(other.itemName);
        }
    }
}