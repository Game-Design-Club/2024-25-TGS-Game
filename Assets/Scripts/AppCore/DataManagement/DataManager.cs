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
        public ScrapbookItem newItem { get; private set; }

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
            newItem = scrapbookItem;
            foundItems.Add(scrapbookItem);
        }

        public void ClearNewItem()
        {
            newItem = null;
        }

        public bool HasScrapbookItem(ScrapbookItem scrapbookItem)
        {
            foreach (ScrapbookItem item in foundItems)
            {
                if (scrapbookItem.Equals(item)) return true;
            }

            return false;
        }
    }

}
