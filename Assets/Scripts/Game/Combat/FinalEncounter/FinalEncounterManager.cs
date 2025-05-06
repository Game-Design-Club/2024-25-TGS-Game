using System.Collections;
using AppCore.DialogueManagement;
using Game.Combat.Enemies.DebrisEnemy;
using Game.Exploration.Child;
using Game.GameManagement;
using Tools;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Combat.FinalEncounter {
    public class FinalEncounterManager : MonoBehaviour {
        [SerializeField] private GameObject enemyWall;
        [SerializeField] private CinemachineCamera followCamera;
        [SerializeField] private Dialogue runDialogue;
        [SerializeField] private GameObject[] setActiveOnStart;
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
        }

        public void StartFinalEncounter(CombatAreaManager manager) {
            runDialogue.StartDialogue(StartMoving);
            _oldCombatArea = manager;
            foreach (GameObject obj in setActiveOnStart) {
                obj.SetActive(true);
            }
        }

        private void StartMoving() {
            GameManager.GameEventType = GameEventType.FinalEncounter;
            enemyWall.SetActive(true);
            followCamera.Priority = 100;
            LevelManager.GetCurrentLevel().child.TransitionToState(new FinalEncounterState(LevelManager.GetCurrentLevel().child));
            _moving = true;
            StartCoroutine(SpawnDebris());
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
        }
    }
}