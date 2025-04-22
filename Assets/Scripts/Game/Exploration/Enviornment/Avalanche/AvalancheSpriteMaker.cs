using Tools;
using Tools.LevelDesign;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Game.Exploration.Enviornment.Avalanche {
    public class AvalancheSpriteMaker : MonoBehaviour {
        [SerializeField] private Transform topLeft;
        [SerializeField] private Transform bottomRight;
        [SerializeField] private GameObject chunkPrefab;
        [SerializeField] private float ySpacing = 0.35f;
        [SerializeField] private float xSpacing = 0.5f;
        [SerializeField] private float positionRandomization = 0.1f;
        
        private void Start() {
            CreateChunks();
        }

        private void CreateChunks() {
            float width = bottomRight.position.x - topLeft.position.x;
            float height = topLeft.position.y - bottomRight.position.y;

            int chunksX = Mathf.CeilToInt(width / xSpacing);
            int chunksY = Mathf.CeilToInt(height / ySpacing);
            
            int totalChunks = chunksX * chunksY;
            List<int> sortingOrders = Enumerable.Range(0, totalChunks)
                                                .Select(i => -100 - i)
                                                .OrderBy(_ => Random.value)
                                                .ToList();
            int orderIndex = 0;

            for (int x = 0; x < chunksX; x++) {
                for (int y = 0; y < chunksY; y++) {
                    Vector3 position = new Vector3(
                        topLeft.position.x + x * xSpacing + positionRandomization.GetRandom(),
                        topLeft.position.y - y * ySpacing + positionRandomization.GetRandom(),
                        0
                    );
                    GameObject obj = Instantiate(chunkPrefab, position, Quaternion.identity, transform);
                    obj.GetComponent<SpriteChooser>().RandomizeSprite();
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null) {
                        sr.sortingOrder = sortingOrders[orderIndex++];
                    }
                }
            }
        }
    }
}
