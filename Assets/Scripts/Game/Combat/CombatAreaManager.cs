using System;
using System.Collections;
using Game.Combat.Bear;
using Game.GameManagement;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Combat {
    public class CombatAreaManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private CinemachineCamera sleepCamera;
        [SerializeField] private BearController bear;
        [Header("Transitions")]
        [SerializeField] private float transitionDuration = 1f;

        private void Awake() {
            bear.gameObject.SetActive(false);
        }

        internal void EnterCombatArea() {
            StartCoroutine(TransitionToCombat());
        }
        
        private IEnumerator TransitionToCombat() {
            GameManager.StartTransitionToCombat();
            sleepCamera.Priority = 100;
            bear.gameObject.SetActive(true);
            yield return new WaitForSeconds(transitionDuration);
            GameManager.EndTransitionToCombat();
        }
    }
}
