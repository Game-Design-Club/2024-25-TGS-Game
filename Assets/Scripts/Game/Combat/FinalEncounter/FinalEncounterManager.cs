using Unity.Cinemachine;
using UnityEngine;

namespace Game.Combat.FinalEncounter {
    public class FinalEncounterManager : MonoBehaviour {
        [SerializeField] private GameObject enemyWall;
        [SerializeField] private CinemachineCamera followCamera;
        public void StartFinalEncounter() {
            enemyWall.SetActive(true);
            followCamera.Priority = 100;
        }
    }
}