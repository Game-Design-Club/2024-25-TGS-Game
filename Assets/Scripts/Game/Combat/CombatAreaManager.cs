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
        [Header("Enemies")]
        [SerializeField] private WavesData wavesData;
        [Header("Combat Area")]
        [SerializeField] private Transform combatAreaSize;
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
        private int _enemiesToKill = 0;
        
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
            
            Setup();

            yield return new WaitForSeconds(GameManager.TransitionDuration);
            GameManager.EndTransitionToCombat();

            StartCoroutine(RunCombat());
        }

        private void Setup() {
            sleepCamera.Priority = 100;

            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(true);
            }
        }

        // Run combat
        private IEnumerator RunCombat() {
            foreach (Wave wave in wavesData.waves) {
                yield return StartCoroutine(SpawnWave(wave));
                yield return new WaitForSeconds(wavesData.bufferBetweenWaves);
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
                yield return new WaitForSeconds(entry.bufferAfterThisEntry);
            }
            _enemiesToKill = wave.GetTotalEnemies();
            yield return new WaitUntil(() => _enemiesToKill == 0);
        }

        private IEnumerator SpawnEntry(WaveEntry entry) {
            foreach (float time in entry.GetSpawnTimes()) {
                yield return new WaitForSeconds(time);
                SpawnEnemy(entry);
            }
        }

        private void SpawnEnemy(WaveEntry entry) {
            Vector2 spawnPos = GetSpawnPosition(entry);
            GameObject enemyObject =  Instantiate(CombatObjectsData.GetEnemyPrefab(entry.enemyType), spawnPos, Quaternion.identity);
            EnemyBase enemy = enemyObject.GetComponent<EnemyBase>();
            enemy.CombatManager = this;
            _activeEnemies.Add(enemy);
        }
        
        private Vector2 GetSpawnPosition(WaveEntry entry) {
            float height = combatAreaSize.localScale.y;
            float width = combatAreaSize.localScale.x;
    
            float totalPositions = 0;
            if (entry.spawnLeft) totalPositions += height;
            if (entry.spawnRight) totalPositions += height;
            if (entry.spawnTop) totalPositions += width;
            if (entry.spawnBottom) totalPositions += width;
            
            float chosenPosition = UnityEngine.Random.Range(0, totalPositions);
    
            Vector2 spawnPos = transform.position;
            
            // -- Left --
            if (entry.spawnLeft) {
                if (chosenPosition < height) {
                    float yPos = chosenPosition - (height / 2);
                    float xPos = (-width / 2);
                    spawnPos += new Vector2(xPos, yPos);
                    return spawnPos;
                }
                chosenPosition -= height;
            }
            
            // -- Right --
            if (entry.spawnTop) {
                if (chosenPosition < width) {
                    float xPos = chosenPosition - (width / 2);
                    float yPos = (height / 2);
                    spawnPos += new Vector2(xPos, yPos);
                    return spawnPos;
                }
                chosenPosition -= width;
            }
            
            // -- Top --
            if (entry.spawnTop) {
                if (chosenPosition < height) {
                    float yPos = chosenPosition - (height / 2);
                    float xPos = (width / 2);
                    spawnPos += new Vector2(xPos, yPos);
                    return spawnPos;
                }
                chosenPosition -= height;
            }
            
            // -- Bottom --
            if (entry.spawnBottom) {
                if (chosenPosition < width) {
                    float xPos = chosenPosition - (width / 2);
                    float yPos = (-height / 2);
                    spawnPos += new Vector2(xPos, yPos);
                    return spawnPos;
                }
            }
            
            return transform.position;
        }
        
        // End combat
        private void PlayerWon() {
            StopAllCoroutines();
            StartCoroutine(TransitionToExploration());
        }
        private IEnumerator TransitionToExploration() {
            GameManager.StartTransitionToExploration();
            
            Cleanup();

            yield return new WaitForSeconds(GameManager.TransitionDuration);
            
            GameManager.EndTransitionToExploration();
            
            _combatEntered = false;
        }

        private void Cleanup() {
            StopAllCoroutines();
            
            sleepCamera.Priority = 0;
            
            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(false);
            }
            
            foreach (EnemyBase enemy in _activeEnemies) {
                enemy.Die();
            }
            
            _activeEnemies.Clear();
        }

        // Internal functions
        internal void EnemyKilled(EnemyBase enemy) {
            _activeEnemies.Remove(enemy);
            _enemiesToKill--;
            Sanity += enemy.sanityRestored;
            if (Sanity >= 100) {
                PlayerWon();
            }
        }
        
        internal void ChildHit(EnemyBase enemy) {
            Sanity -= enemy.sanityDamage;
            _activeEnemies.Remove(enemy);
            _enemiesToKill--;
            if (Sanity <= 0) {
                PlayerLost();
            }
        }

        private void PlayerLost() {
            Debug.Log("DEAD");
        }

        // Helper functions
        private float GetSanityPercentage() {
            return (Sanity - loseSanityThreshold) / (winSanityThreshold - loseSanityThreshold);
        }
    }
}
