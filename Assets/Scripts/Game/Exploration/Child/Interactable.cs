using System;
using UnityEngine;

namespace Game.Exploration.Child {
    public interface Interactable {
        public  void Interact(Action overCallback);
    }
}