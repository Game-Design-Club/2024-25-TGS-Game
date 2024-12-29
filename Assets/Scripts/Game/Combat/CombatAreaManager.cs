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
        [Header("Enemies")]
        [SerializeField] private Wave[] waves;
        [Header("Sanity")]
        [SerializeField] private float winSanityThreshold = 100f;
        [SerializeField] private float loseSanityThreshold = 0f;
        [SerializeField] private float startInsanity = 20f;

        // Private fields
        private float _sanity = 20f; // 0 - 100
        
        private bool _combatEntered = false;
        
        // Events
        public static event Action<float> OnSanityChanged; // Percentage

        private void Awake() {
            bear.gameObject.SetActive(false);
        }
        
        // Start combat
        internal void EnterCombatArea() {
            StartCoroutine(TransitionToCombat());
        }
        
        private IEnumerator TransitionToCombat() {
            if (_combatEntered) yield break;
            _combatEntered = true;
            
            GameManager.StartTransitionToCombat();
            
            sleepCamera.Priority = 100;
            bear.gameObject.SetActive(true);
            
            
            yield return new WaitForSeconds(transitionDuration);
            GameManager.EndTransitionToCombat();

            StartCoroutine(RunCombat());
        }
        
        // Run combat
        private IEnumerator RunCombat() {
            foreach (Wave wave in waves) {
                yield return StartCoroutine(SpawnWave(wave));
            }
        }
        
        private IEnumerator SpawnWave(Wave wave) {
            Instantiate(wave.enemyPrefab, wave.spawnPoint.position, Quaternion.identity);
            yield return null;
        }
        
        // End combat
        private IEnumerator TransitionToExploration() {
            GameManager.StartTransitionToExploration();
            
            sleepCamera.Priority = 0;
            bear.gameObject.SetActive(false);
            
            yield return new WaitForSeconds(transitionDuration);
            GameManager.EndTransitionToExploration();
            
            _combatEntered = false;
        }
        
        // Public functions
        public void MonsterKilled(float sanity) {
            _sanity = Mathf.Clamp(_sanity + sanity, loseSanityThreshold, winSanityThreshold);
            OnSanityChanged?.Invoke(GetSanityPercentage());
            if (sanity >= 100) {
                StartCoroutine(TransitionToExploration());
            }
        }
        
        // Helper functions
        private float GetSanityPercentage() {
            return (_sanity - loseSanityThreshold) / (winSanityThreshold - loseSanityThreshold);
        }
    }
}
