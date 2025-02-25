using AppCore.DialogueManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables.Scrapbook
{
    [CreateAssetMenu(fileName = "New Scrapbook Item", menuName = "Scrapbook/Item")]
    public class ScrapbookItem : ScriptableObject
    {
        [SerializeField] public string name;
        [SerializeField] public Sprite sprite;
        [SerializeField] public Dialogue dialogue;
        [SerializeField] public string description;
    }
}