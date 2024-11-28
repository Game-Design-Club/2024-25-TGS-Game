using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Generation_System
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private GameObject worldManagerObject;
        [SerializeField] private GameObject chunkPrefab;
        [SerializeField] private BasicObject tree;
        
        [SerializeField] private Vector2Int center = new Vector2Int(0, 0);
        [SerializeField] private float size = 10;
        [SerializeField] private int loadedDistance = 5;

        private Dictionary<String, Chunk> _chunks = new Dictionary<string, Chunk>();
        private List<Chunk> _loadedChunks = new List<Chunk>();


        // Update is called once per frame
        void Update()
        {
            UpdateChunks();
        }

        private void UpdateChunks()
        {
            //load chunks if needed (and create if needed)
            for (int x = center.x - loadedDistance; x <= center.x + loadedDistance; x++)
            {
                for (int y = center.y - loadedDistance; y <= center.y + loadedDistance; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    String key = Chunk.GetKey(pos);
                    if (Vector2Int.Distance(pos, center) > loadedDistance) continue;
                    if (!_chunks.ContainsKey(key)) CreateChunk(pos);

                    Chunk chunk = _chunks[key];
                    
                    if (_loadedChunks.Contains(chunk)) continue;

                    GameObject o = Instantiate(chunkPrefab, new Vector3(pos.x * size, pos.y * size, 0), new Quaternion(0, 0, 0, 0), worldManagerObject.transform);
                    o.name = $"Chunk {Chunk.GetKey(pos)}";
                    chunk.Load(o, tree, size);
                    _loadedChunks.Add(chunk);
                }
            }

            List<Chunk> chunksToUnload = new List<Chunk>();
            // String s = "Current Loaded Chunks: \n";
            //update chunks
            foreach (Chunk chunk in _loadedChunks)
            {
                if (Vector2Int.Distance(chunk.Position, center) > loadedDistance) chunksToUnload.Add(chunk);
                // s += $"{chunk.GetKey()}\n";
            }
            // Debug.Log(s);
            
            //unload chunks if needed
            foreach (Chunk chunk in chunksToUnload)
            {
                chunk.Unload();
                _loadedChunks.Remove(chunk);
            }
        }

        private void CreateChunk(Vector2Int position)
        {
            _chunks.Add(Chunk.GetKey(position), new Chunk(position));
        }
    }
}
