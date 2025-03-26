using Game.Exploration.Child;
using UnityEngine;

namespace Game.GameManagement {
    public class Level : MonoBehaviour {
        [SerializeField] public ChildController child;
        [SerializeField] public Transform spawnPoint;
    }
}