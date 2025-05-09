using Game.Exploration.Enviornment.River;
using UnityEngine;

namespace Tools {
    public class PointCollision {
        public readonly Collider2D[] Colliders = new Collider2D[10];
        public Vector2 Point;
        public bool TouchingRiver => TouchingRiverBase || (TouchingRiverAddition && TouchingRiverBase);
        public bool TouchingLand => TouchingGround || TouchingRock || TouchingLog;
        public bool TouchingRiverAddition = false;
        public bool TouchingRiverBase = false;
        public bool TouchingRock = false;
        public bool TouchingLog = false;
        public bool TouchingGround = false;
        public RiverChunk RiverAddition;
        public RiverChunk RiverBase;
        public RiverChunk River => TouchingRiverAddition ? RiverAddition : RiverBase;
        public RiverManager RiverManager;

        public PointCollision(Vector2 point, Collider2D ignoreCollider = null) {
            Point = point;
            Physics2D.OverlapPoint(Point, new ContactFilter2D().NoFilter(), Colliders);
                
            foreach (Collider2D collider in Colliders) {
                if (collider is null || collider == ignoreCollider) {
                    continue;
                }
                if (collider.CompareTag(Tags.Rock)) {
                    if (collider.GetComponent<RiverRock>().InRiver) {
                        TouchingRock = true;
                    }
                } else if (collider.CompareTag(Tags.Log)) {
                    TouchingLog = true;
                } else if (collider.CompareTag(Tags.River)) {
                    RiverAddition = collider.GetComponent<RiverChunk>();
                    TouchingRiverAddition = true;
                } else if (collider.CompareTag(Tags.RiverBase)) {
                    RiverBase = collider.GetComponent<RiverChunk>();
                    RiverManager = collider.GetComponentInParent<RiverManager>();
                    TouchingRiverBase = true;
                }
            }
            
            if (!TouchingRiverBase && !TouchingRock && !TouchingLog) {
                TouchingGround = true;
            }
        }

        public override string ToString() {
            string result = $"Point: {Point}\n";
            result += $"Touching River Base: {TouchingRiverBase}\n";
            result += $"Touching River Addition: {TouchingRiverAddition}\n";
            result += $"Touching Rock: {TouchingRock}\n";
            result += $"Touching Log: {TouchingLog}\n";
            result += $"Touching Ground: {TouchingGround}\n";
            return result;
        }
    }
}