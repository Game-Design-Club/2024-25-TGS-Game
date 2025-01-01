using System;
using System.Collections;
using System.Collections.Generic;
using Game.Combat.Enemies;
using Game.Exploration.Child;
using Game.GameManagement;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat {
    public class CombatAreaManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private CinemachineCamera sleepCamera;
        [SerializeField] private List<GameObject> activeStateSwitchOnCombat;
        [Header("Transitions")]
        [SerializeField] private float transitionDuration = 1f;
        [FormerlySerializedAs("waves")]
        [Header("Enemies")]
        [SerializeField] private WavesData wavesData;
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
        private List<EnemyBase> _activeEnemies = new();
        private int enemiesToKill = 0;
        
        internal ChildController Child;
        
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
        internal void EnterCombatArea(ChildController child) {
            Child = child;
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
            foreach (Wave wave in wavesData.waves) {
                yield return new WaitForSeconds(wavesData.bufferBetweenWaves);
                yield return StartCoroutine(SpawnWave(wave));
            }

            // Keep going until sanity is 100, repeat waves with canReplay
            // while (true) {
            //     foreach (Wave wave in wavesData.waves) {
            //         if (!wave.canReplay) continue;
            //         yield return new WaitForSeconds(wavesData.bufferBetweenWaves);
            //         yield return StartCoroutine(SpawnWave(wave));
            //     }
            // }
        }

        private IEnumerator SpawnWave(Wave wave) {
            foreach (WaveEntry entry in wave.waveEntries) {
                StartCoroutine(SpawnEntry(entry));
            }
            enemiesToKill += wave.GetTotalEnemies();
            yield return new WaitUntil(() => _activeEnemies.Count == 0);
        }

        private IEnumerator SpawnEntry(WaveEntry entry) {
            foreach (float time in entry.GetSpawnTimes()) {
                yield return new WaitForSeconds(time);
                SpawnEnemy(entry);
            }
        }

        private void SpawnEnemy(WaveEntry entry) {
            GameObject enemyObject =  Instantiate(CombatObjectsData.GetEnemyPrefab(entry.enemyType), GetSpawnPosition(entry), Quaternion.identity);
            EnemyBase enemy = enemyObject.GetComponent<EnemyBase>();
            enemy.CombatManager = this;
            _activeEnemies.Add(enemy);
        }
        
        private Vector2 GetSpawnPosition(WaveEntry entry) {
            float spawnHeight = 5;
            float spawnWidth = 10;
    
            float totalPositions = 0;
            if (entry.spawnLeft) totalPositions += spawnHeight;
            if (entry.spawnRight) totalPositions += spawnHeight;
            if (entry.spawnTop) totalPositions += spawnWidth;
            if (entry.spawnBottom) totalPositions += spawnWidth;
            
            float chosenPosition = UnityEngine.Random.Range(0, totalPositions);
    
            Vector2 spawnPos;
            // if (chosenPosition < worldWidth) {
            //     spawnPos = new Vector2(chosenPosition - worldWidth / 2, worldHeight / 2 - spawnBuffer);
            // } else if (chosenPosition < worldWidth + worldHeight) {
            //     spawnPos = new Vector2(worldWidth / 2 - spawnBuffer, chosenPosition - worldWidth - worldHeight / 2);
            // } else if (chosenPosition < worldWidth * 2 + worldHeight) {
            //     spawnPos = new Vector2(chosenPosition - worldWidth - worldHeight - worldWidth / 2, -worldHeight / 2 + spawnBuffer);
            // } else {
            //     spawnPos = new Vector2(-worldWidth / 2 + spawnBuffer, chosenPosition - worldWidth * 2 - worldHeight - worldHeight / 2);
            // }
            
            return transform.position;
        }
        
        // End combat
        private IEnumerator TransitionToExploration() {
            StopAllCoroutines();
            
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
        internal void EnemyKilled(EnemyBase enemy) {
            _activeEnemies.Remove(enemy);
            Sanity = Mathf.Clamp(Sanity + enemy.sanityRestored, loseSanityThreshold, winSanityThreshold);
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
