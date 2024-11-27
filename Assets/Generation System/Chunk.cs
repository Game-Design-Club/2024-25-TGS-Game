using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generation_System
{
    public class Chunk
    {
        internal Vector2Int Position;
        internal GameObject tempObject;
        
        internal Chunk(Vector2Int position)
        {
            Position = position;
            Debug.Log($"Created Chunk {GetKey()}");
        }

        internal void Load(GameObject tempObject)
        {
            this.tempObject = tempObject;
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