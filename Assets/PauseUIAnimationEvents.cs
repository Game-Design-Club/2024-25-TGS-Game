using System;
using UnityEngine;

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
