using System;
using System.Collections;
using System.Collections.Generic;
using Game.Combat.Bear;
using Game.Combat.Enemies;
using Game.GameManagement;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Combat {
    public class CombatAreaManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private CinemachineCamera sleepCamera;
        [SerializeField] private List<GameObject> activeStateSwitchOnCombat;
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
        private float Sanity {
            get => _sanity;
            set {
                _sanity = Mathf.Clamp(value, loseSanityThreshold, winSanityThreshold);
                OnSanityChanged?.Invoke(GetSanityPercentage());
            }
        }
        
        private bool _combatEntered = false;
        
        // Events
        public static event Action<float> OnSanityChanged; // Percentage

        private void Awake() {
            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(false);
            }
        }

        private void Start() {
            Sanity = startInsanity;
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
            
            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(true);
            }
            
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
            GameObject enemy =  Instantiate(wave.enemyPrefab, wave.spawnPoint.position, Quaternion.identity);
            enemy.GetComponent<EnemyBase>().CombatManager = this;
            yield return null;
        }
        
        // End combat
        private IEnumerator TransitionToExploration() {
            GameManager.StartTransitionToExploration();
            
            sleepCamera.Priority = 0;
            
            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(false);
            }
            
            yield return new WaitForSeconds(transitionDuration);
            GameManager.EndTransitionToExploration();
            
            _combatEntered = false;
        }
        
        // Internal functions
        internal void EnemyKilled(float sanityRestored) {
            Sanity = Mathf.Clamp(Sanity + sanityRestored, loseSanityThreshold, winSanityThreshold);
            if (Sanity >= 100) {
                StartCoroutine(TransitionToExploration());
            }
        }
        
        // Helper functions
        private float GetSanityPercentage() {
            return (Sanity - loseSanityThreshold) / (winSanityThreshold - loseSanityThreshold);
        }
    }
}
