using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Exploration.Enviornment.Avalanche {
    public class AvalancheRockSpawner : MonoBehaviour {
        [SerializeField] private GameObject rockPrefab;
        [SerializeField] private AnimationCurve spawnRateCurveMin;
        [SerializeField] private AnimationCurve spawnRateCurveMax;
        [SerializeField] private float range = 5;
        [SerializeField] private float rockSpeed = 5;
        [SerializeField] private Transform avalancheBottom;
        [SerializeField] private float spawnRandomness = 0.5f;
        
        private AvalancheManager _avalancheManager;
        
        private List<GameObject> _spawnedRocks = new();
        
        internal void StartSpawning(AvalancheManager manager) {
            StopAllCoroutines();
            _avalancheManager = manager;
            StartCoroutine(SpawnRocks());
        }

        private IEnumerator SpawnRocks() {
            while (true) {
                float y = transform.position.y;
                if (avalancheBottom.position.y < y) {
                    y = avalancheBottom.position.y;
                }
                Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-range, range), y, 0);
                
                GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
                _spawnedRocks.Add(rock);
                rock.GetComponent<AvalancheRock>().Launch(rockSpeed);
                float percentCovered = _avalancheManager.percentCovered;
                yield return new WaitForSeconds(Random.Range(
                    spawnRateCurveMin.Evaluate(percentCovered),
                    spawnRateCurveMax.Evaluate(percentCovered)
                    ) + Random.Range(-spawnRandomness, spawnRandomness));
            }
        }

        internal void StopSpawning() {
            StopAllCoroutines();
            foreach (GameObject rock in _spawnedRocks) {
                if (rock != null) {
                    Destroy(rock);
                }
            }
        }
    }
}