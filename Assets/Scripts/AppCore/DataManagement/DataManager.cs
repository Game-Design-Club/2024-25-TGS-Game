using System.Collections;
using Game.Exploration.Enviornment.Interactables.Scrapbook;
using UnityEngine;

namespace AppCore.DataManagement
{
    public class DataManager : AppModule
    {
        public bool HasStick { get; private set; }
        public bool UnlockedAttack { get; private set; }

        public ArrayList foundItems { get; private set; } = new ArrayList();

        public void ObtainedStick()
        {
            HasStick = true;
            UnlockAttack();
            Debug.Log("Has Stick");
        }
        
        public void UnlockAttack()
        {
            UnlockedAttack = true;
            Debug.Log("Can Attack");
        }

        public void FoundScrapbookItem(ScrapbookItem scrapbookItem)
        {
            foundItems.Add(scrapbookItem);
        }
    }

}
