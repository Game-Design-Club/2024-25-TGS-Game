using System;
using UnityEngine;

namespace Game.Exploration.UI
{
    public class PauseUIAnimationEvents : MonoBehaviour
    {
        public event Action OnBookUp;
        public event Action OnBookDown;

        public void OnBookUpAnimationFinished()
        {
            OnBookUp?.Invoke();
        }
    
        public void OnBookDownAnimationFinished()
        {
            OnBookDown?.Invoke();
        }
    }
}
