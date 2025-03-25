using Game.Exploration.Enviornment.River;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class PlayerPointCollision {
        public readonly Collider2D[] Colliders = new Collider2D[10];
        public Vector2 Point;
        public bool TouchingRiver => TouchingRiverBase || (TouchingRiverAddition && TouchingRiverBase);
        public bool TouchingLand => TouchingGround || TouchingRock || TouchingLog;  
        public bool TouchingRiverAddition = false;
        public bool TouchingRiverBase = false;
        public bool TouchingRock = false;
        public bool TouchingLog = false;
        public bool TouchingGround = false;
        public RiverChunk River;

        public PlayerPointCollision(Vector2 point) {
            Point = point;
            Physics2D.OverlapPoint(Point, new ContactFilter2D().NoFilter(), Colliders);
                
            foreach (Collider2D collider in Colliders) {
                if (collider is null) {
                    continue;
                }
                if (collider.CompareTag(Tags.Rock)) {
                    TouchingRock = true;
                } else if (collider.CompareTag(Tags.Log)) {
                    TouchingLog = true;
                } else if (collider.CompareTag(Tags.River)) {
                    River = collider.GetComponent<RiverChunk>();
                    TouchingRiverAddition = true;
                } else if (collider.CompareTag(Tags.RiverBase)) {
                    River = collider.GetComponent<RiverChunk>();
                    TouchingRiverBase = true;
                }
            }
            
            if (!TouchingRiverAddition && !TouchingRiverBase && !TouchingRock && !TouchingLog) {
                TouchingGround = true;
            }
        }
    }
}