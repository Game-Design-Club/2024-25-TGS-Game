using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generation_System
{
    public class Chunk
    {
        internal Vector2Int Position;
        internal GameObject tempObject;
        internal Vector2[] basicObjects;
        
        internal Chunk(Vector2Int position)
        {
            Position = position;
            basicObjects = new Vector2[10];
            for (int i = 0; i < 10; i++)
            {
                basicObjects[i] = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
            Debug.Log($"Created Chunk {GetKey()}");
        }

        internal void Load(GameObject tempObject, BasicObject o, float size)
        {
            this.tempObject = tempObject;
            foreach (Vector2 pos in basicObjects)
            {
                UnityEngine.Object.Instantiate(o.gameObject, new Vector3((Position.x + pos.x) * size - size / 2.0f, (Position.y + pos.y) * size - size / 2.0f, 0), new Quaternion(0, 0, 0, 0), tempObject.transform);
            }
            Debug.Log($"Loaded Chunk {GetKey()}");
        }

        internal void Unload()
        {
            UnityEngine.Object.Destroy(tempObject);
            Debug.Log($"Unloaded Chunk {GetKey()}");
        }

        internal String GetKey()
        {
            return GetKey(Position);
        }
        
        internal static String GetKey(Vector2Int p)
        {
            return $"({p.x}, {p.y})";
        }
    }
    
}