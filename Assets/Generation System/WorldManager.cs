using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generation_System
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int _center = new Vector2Int(0, 0);
        [SerializeField] private float _size = 10;
        [SerializeField] private int _loadedDistance = 5;
        
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
            for (int x = _center.x - _loadedDistance; x <= _center.x + _loadedDistance; x++)
            {
                for (int y = _center.y - _loadedDistance; y <= _center.y + _loadedDistance; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    String key = Chunk.GetKey(pos);
                    if (Vector2Int.Distance(pos, _center) > _loadedDistance) continue;
                    if (!_chunks.ContainsKey(key)) CreateChunk(pos);

                    Chunk chunk = _chunks[key];
                    
                    if (_loadedChunks.Contains(chunk)) continue;
                    
                    chunk.Load();
                    _loadedChunks.Add(chunk);
                }
            }

            List<Chunk> chunksToUnload = new List<Chunk>();
            String s = "Current Loaded Chunks: \n";
            //update chunks
            foreach (Chunk chunk in _loadedChunks)
            {
                if (Vector2Int.Distance(chunk.Position, _center) > _loadedDistance) chunksToUnload.Add(chunk);
                s += $"{chunk.GetKey()}\n";
            }
            Debug.Log(s);
            
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
