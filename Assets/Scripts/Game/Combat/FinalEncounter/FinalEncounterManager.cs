using System.Collections;
using System.Collections.Generic;
using AppCore.DialogueManagement;
using Game.Combat.Enemies.DebrisEnemy;
using Game.Exploration.Child;
using Game.GameManagement;
using Tools;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Combat.FinalEncounter {
    public class FinalEncounterManager : MonoBehaviour {
        [SerializeField] private GameObject enemyWall;
        [SerializeField] private CinemachineCamera followCamera;
        [SerializeField] private Dialogue runDialogue;
        [SerializeField] private GameObject[] setActiveOnStart;
        [SerializeField] private float dieResetTime = 1f;
        [SerializeField] private float fadeTime = .3f;
        [SerializeField] private float fadePauseTime = 0.3f;
        [SerializeField] private Image fadeImage;
        [SerializeField] private Transform playerStartPos;
        [SerializeField] private GameObject[] prototypes;
        private List<GameObject> activeClones = new List<GameObject>();
        private int originalCameraPriority;
        private Coroutine debrisCoroutine;
        [Header("Movement")]
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform movePoint;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float behindMoveSpeed = 4f;
        [SerializeField] private Transform cameraSnapEdge;
        [Header("Debris Enemy")]
        [SerializeField] private FloatRange debrisSpawnSpeed = new FloatRange(1f, 2f);
        [SerializeField] private float debrisSpawnHeight = 3f;
        [SerializeField] private Transform debrisSpawnPoint;

        private bool _moving;
        private float _coveredDistance;
        private float _percentCovered;
        private float _totalDistance;
        private Vector2 _startPos;
        private Vector2 _endPos;

        private bool _resetting = false;
        
        private CombatAreaManager _oldCombatArea;

        private void Awake() {
            enemyWall.SetActive(false);
            _totalDistance = Vector2.Distance(startPoint.position, endPoint.position);
            _startPos = startPoint.position;
            _endPos = endPoint.position;
            Destroy(startPoint.gameObject);
            Destroy(endPoint.gameObject);
            foreach (GameObject obj in setActiveOnStart) {
                obj.SetActive(false);
            }

            // Ensure prototype objects start disabled
            foreach (var proto in prototypes) {
                proto.SetActive(false);
            }
        }

        public void StartFinalEncounter(CombatAreaManager manager) {
            runDialogue.StartDialogue(StartMoving);
            _oldCombatArea = manager;
            foreach (GameObject obj in setActiveOnStart) {
                obj.SetActive(true);
            }

            // Spawn fresh instances of level geometry
            foreach (var proto in prototypes) {
                var clone = Instantiate(proto, proto.transform.position, proto.transform.rotation);
                activeClones.Add(clone);
                clone.SetActive(true);
            }
        }

        private void StartMoving() {
            GameManager.GameEventType = GameEventType.FinalEncounter;
            enemyWall.SetActive(true);
            // Cache and bump camera
            originalCameraPriority = followCamera.Priority;
            followCamera.Priority = 100;
            LevelManager.GetCurrentLevel().child.TransitionToState(new FinalEncounterState(LevelManager.GetCurrentLevel().child));
            _moving = true;
            // Launch debris spawn coroutine
            debrisCoroutine = StartCoroutine(SpawnDebris());
        }
        
        private void Update() {
            if (_moving) {
                float movement = Time.deltaTime;
                Camera cam = Camera.main;
                Vector3 p = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
                if (cameraSnapEdge.position.x < p.x) {
                    movement *= behindMoveSpeed;
                } else {
                    movement *= moveSpeed;
                }
                _coveredDistance += movement;
                _percentCovered = _coveredDistance / _totalDistance;
                _percentCovered = Mathf.Clamp01(_percentCovered);
                movePoint.position = Vector2.Lerp(_startPos, _endPos, _percentCovered);
                
                if (_percentCovered >= 1f) {
                    StopCombat();
                }
            }
        }
        
        private IEnumerator SpawnDebris() {
            while (_moving) {
                yield return new WaitForSeconds(debrisSpawnSpeed.Random());
                Vector2 spawnPos = (Vector2)debrisSpawnPoint.position + new Vector2(0, Random.Range(-debrisSpawnHeight/2f, debrisSpawnHeight/2f));
                GameObject enemyObject =  Instantiate(CombatObjectsData.GetEnemyPrefab(EnemyType.Debris), spawnPos, Quaternion.Euler(0,0,90));
                DebrisEnemy enemy = enemyObject.GetComponent<DebrisEnemy>();
                enemy.horizontalMovement = true;
            }
        }

        private void StopCombat() {
            _moving = false;
            if (debrisCoroutine != null) StopCoroutine(debrisCoroutine);
            followCamera.Priority = originalCameraPriority;
            LevelManager.GetCurrentLevel().child.TransitionToState(new Move(LevelManager.GetCurrentLevel().child));
        }

        public void ChildHit() {
            if (_resetting) return;
            _resetting = true;
            LevelManager.GetCurrentLevel().child.Sleep(default);
            StopAllCoroutines();
            StartCoroutine(WaitToDie());
            
        }

        private IEnumerator WaitToDie() {
            yield return new WaitForSeconds(fadePauseTime);

            float t = 0;
            while (t < fadeTime) {
                t += Time.deltaTime;
                float percent = t / fadeTime;
                fadeImage.color = new Color(0,0,0,percent);
                yield return null;
            }
            yield return new WaitForSeconds(dieResetTime);

            ResetEncounter();
            t = 0;
            while (t < fadeTime) {
                t += Time.deltaTime;
                float percent = t / fadeTime;
                fadeImage.color = new Color(0,0,0,1 - percent);
                yield return null;
            }
            _resetting = false;
        }

        private void ResetEncounter(){
            // Destroy all spawned clones
            foreach (var go in activeClones) {
                Destroy(go);
            }
            activeClones.Clear();

            // Reset movement state
            _coveredDistance = 0;
            _percentCovered = 0;
            _moving = true;

            // Reposition player
            LevelManager.GetCurrentLevel().child.transform.position = playerStartPos.position;
            LevelManager.GetCurrentLevel().child.TransitionToState(new FinalEncounterState(LevelManager.GetCurrentLevel().child));

            // Spawn clones again
            foreach (var proto in prototypes) {
                var clone = Instantiate(proto, proto.transform.position, proto.transform.rotation);
                activeClones.Add(clone);
                clone.SetActive(true);
            }
        }
    }
}