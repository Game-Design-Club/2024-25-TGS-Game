using UnityEngine;

namespace AppCore.DataManagement
{
    public class DataManager : AppModule
    {
        public bool HasStick { get; private set; }
        public bool UnlockedAttack { get; private set; }

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
    }
}
