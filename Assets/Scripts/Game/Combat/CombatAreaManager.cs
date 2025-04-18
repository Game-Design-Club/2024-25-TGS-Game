using System;
using System.Collections;
using System.Collections.Generic;
using AppCore.AudioManagement;
using Game.Combat.Bear;
using Game.Combat.Enemies;
using Game.Combat.Waves;
using Game.Exploration.Child;
using Game.GameManagement;
using Tools;
using Tools.CameraShaking;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Combat {
    public class CombatAreaManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private CinemachineCamera wideCamera;
        [SerializeField] private CinemachineCamera combatCamera;
        [SerializeField] private List<GameObject> activeStateSwitchOnCombat;
        [SerializeField] internal CameraShaker cameraShaker;
        [SerializeField] private BearController bearController;
        [Header("Cutscene")]
        [SerializeField] private SoundEffect breathSound;
        [SerializeField] private SoundEffect heartBeatSound;
        [Header("Enemies")]
        [SerializeField] private WavesData wavesData;
        [Header("Combat Area")]
        [SerializeField] private Transform combatAreaSize;
        [SerializeField] private Transform childRestPoint;
        [Header("Sanity")]
        [SerializeField] private float maxSanity = 100f;
        [SerializeField] private float loseSanityThreshold = 0f;
        [SerializeField] private float startInsanity = 20f;
        
        // Private fields
        public float _sanity = 0;
        private float Sanity {
            get => _sanity;
            set {
                _sanity = Mathf.Clamp(value, loseSanityThreshold, maxSanity);
                OnSanityChanged?.Invoke(GetSanityPercentage());
            }
        }
        
        public float SanityPercentage => GetSanityPercentage();

        private bool _combatEntered = false;
        private List<EnemyBase> _activeEnemies = new();
        private int _enemiesToKill = 0;
        
        private bool _lost = false;
        
        internal ChildController Child;

        private Animator _animator;
        
        // Events
        public static event Action<float> OnSanityChanged; // Percentage
        public static event Action OnChildHit;
        
        private void Awake() {
            if (wavesData == null) {
                Debug.LogError("wavesData is null");
            }
            
            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(false);
            }

            TryGetComponent(out _animator);
        }

        private void OnEnable() {
            UIManager.OnRestartGame += RestartCombat;
        }
        
        private void OnDisable() {
            UIManager.OnRestartGame -= RestartCombat;
        }

        // Start combat
        internal void EnterCombatArea(ChildController child) {
            if (_combatEntered) {
                return;
            }
            _combatEntered = true;
            Child = child;
            Sanity = startInsanity;
            TransitionToCombat();
        }
        
        private void TransitionToCombat() {
            GameManager.StartTransitionToCombat();
            
            Setup();
            
            _animator.SetTrigger(AnimationConstants.CombatArea.EnterCombat);
        }
        
        // Animation events
        public void AnimSleepChild() {
            Child.Sleep(childRestPoint.position);
        }

        public void AnimStartCombatRun() {
            StartCoroutine(RunCombat());
        }

        public void AnimSetCombatCamera() {
            combatCamera.Priority = 200;
        }

        public void AnimAwakeBear() {
            bearController.gameObject.SetActive(true);
        }
        
        public void AnimStartBreathing() {
            breathSound.Play();
        }
        
        public void AnimStartHeartbeat() {
            heartBeatSound.Play();
        }
        
        // Animation events
        private IEnumerator TransitionToExploration() {
            GameManager.StartTransitionToExploration();
            
            Cleanup();

            yield return new WaitForSeconds(GameManager.TransitionDuration);
            
            GameManager.EndTransitionToExploration();
            
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
        
        private void Setup() {
            wideCamera.Priority = 100;

            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(true);
            }
        }

        // Run combat
        private IEnumerator RunCombat() {
            _lost = false;

            GameManager.EndTransitionToCombat();
            
            foreach (Wave wave in wavesData.waves) {
                yield return StartCoroutine(SpawnWave(wave));
                yield return new WaitForSeconds(wavesData.bufferBetweenWaves);
            }
            
            PlayerWon();
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
                if (!entry.spawnLeft && !entry.spawnRight && !entry.spawnTop && !entry.spawnBottom) {
                    Debug.LogError("No spawn points for enemy");
                    return transform.position;
                }
                return SpawnFromSides(entry);
        }
        
        private Vector2 SpawnFromSides(WaveEntry entry) {
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
            
            // -- Top --
            if (entry.spawnTop) {
                if (chosenPosition < width) {
                    float xPos = chosenPosition - (width / 2);
                    float yPos = (height / 2);
                    spawnPos += new Vector2(xPos, yPos);
                    return spawnPos;
                }
                chosenPosition -= width;
            }
            
            // -- Right --
            if (entry.spawnRight) {
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
        private void Cleanup() {
            wideCamera.Priority = 0;
            combatCamera.Priority = 0;
            
            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(false);
            }
            foreach (EnemyBase enemy in _activeEnemies) {
                Destroy(enemy.gameObject);
            }
            
            _activeEnemies.Clear();
        }

        // Internal functions
        internal void EnemyKilled(EnemyBase enemy) {
            _activeEnemies.Remove(enemy);
            _enemiesToKill--;
            Sanity += enemy.sanityRestored;
        }
        
        internal void ChildHit(EnemyDamageDealer enemy) {
            if (_lost) return;
            OnChildHit?.Invoke();
            Sanity -= enemy.sanityDamage;
            if (Sanity <= 0) {
                PlayerLost();
            }
        }
        
        internal void RemoveEnemy(EnemyBase enemy) {
            _activeEnemies.Remove(enemy);
            _enemiesToKill--;
        }

        private void PlayerLost() {
            _lost = true;
            GameManager.OnBearDeath();
            StopAllCoroutines();
        }
        
        internal void RestartCombat() {
            StartCoroutine(RestartCombatRoutine());
        }
        
        private IEnumerator RestartCombatRoutine() {
            foreach (GameObject obj in activeStateSwitchOnCombat) {
                obj.SetActive(true);
            }
            foreach (EnemyBase enemy in _activeEnemies) {
                Destroy(enemy.gameObject);
            }
            
            _activeEnemies.Clear();
            GameManager.StartTransitionToCombat();
            Sanity = startInsanity;
            yield return new WaitForSeconds(GameManager.TransitionDuration);
            StartCoroutine(RunCombat());
        }

        // Helper functions
        private float GetSanityPercentage() {
            return (Sanity - loseSanityThreshold) / (maxSanity - loseSanityThreshold);
        }
    }
}
